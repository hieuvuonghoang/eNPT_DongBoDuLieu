using eNPT_DongBoDuLieu.Models;
using eNPT_DongBoDuLieu.Models.Services;
using eNPT_DongBoDuLieu.Services.DataBases;
using eNPT_DongBoDuLieu.Services.Datas;
using eNPT_DongBoDuLieu.Services.Portals;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly AppSettings _appSettings;
        private readonly IDataServices _dataServices;
        private readonly IPortalServices _portalServices;
        private readonly IDataBaseServices _dataBaseServices;

        public Worker(
            ILogger<Worker> logger, 
            IOptions<AppSettings> appSettings,
            IDataServices dataServices,
            IPortalServices portalServices,
            IDataBaseServices dataBaseServices)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
            _dataServices = dataServices;
            _portalServices = portalServices;
            _dataBaseServices = dataBaseServices;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Started.");
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //Get Token
                    var resultGenToken = await _portalServices.GenerateTokenAsync();
                    //Read Data LastEditDate From File
                    var lastEditDate = await _dataServices.ReadDataAsync();
                    _logger.LogInformation($"Khoảng thời gian thực hiện truy vấn: [{lastEditDate.NgayGio.ToString("yyyy-MM-dd HH:mm:ss")}, " +
                        $"{lastEditDate.NgayGioHienTai.ToString("yyyy-MM-dd HH:mm:ss")}]");
                    //Get MaxRecordPerPage
                    var maxRecordPerPage = _appSettings.MaxRecordPerPage;
                    //Đồng bộ dữ liệu Đường dây điện
                    await RunAsync(resultGenToken.token, ELoaiDT.DDA, lastEditDate);
                    //Đồng bộ dữ liệu Trạm biến áp
                    await RunAsync(resultGenToken.token, ELoaiDT.TBA, lastEditDate);
                    //Đồng bộ dữ liệu Cột điện
                    await RunAsync(resultGenToken.token, ELoaiDT.COT, lastEditDate);
                    //Write Data LastEditDate To File
                    await _dataServices.WriteDataAsync(lastEditDate);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message, ex);
                }
                finally
                {
                    await Task.Delay(_appSettings.RefreshMilliSeconds, stoppingToken);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service Stopped.");
            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// Hàm thực hiện đồng bộ dữ liệu từng loại đối tượng.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="loaiDT"></param>
        /// <param name="lastEditDate"></param>
        /// <returns></returns>
        /// <exception cref="Không ném ra bất kỳ ngoại lệ nào. Chỉ ghi log Err."></exception>
        private async Task RunAsync(string token, ELoaiDT loaiDT, LastEditDate lastEditDate)
        {
            try
            {
                _logger.LogInformation($"Thực hiện đồng bộ dữ liệu đối tượng {loaiDT}...");
                var countRecord = await _portalServices.GetCountByLastEditTimeAsync(token, loaiDT, lastEditDate);
                var maxRecordPerPage = _appSettings.MaxRecordPerPage;
                if (countRecord != 0)
                {
                    _logger.LogInformation($"Loại đối tượng {loaiDT}. Có {countRecord} bản ghi được cập nhật.");
                    var nPage = countRecord / maxRecordPerPage;
                    if (countRecord % _appSettings.MaxRecordPerPage != 0)
                    {
                        nPage++;
                    }
                    var resultOffset = 0;
                    for (var i = 0; i < nPage; i++)
                    {
                        _logger.LogInformation($"Đang đồng bộ dữ liệu loại đối tượng {loaiDT} trang {i + 1} trên tổng số {nPage} trang...");
                        var strFeatures = await _portalServices.GetFeatureAsyncs(token, loaiDT, lastEditDate, resultOffset, maxRecordPerPage);
                        await _dataBaseServices.DeleteAndInsertFullTextSearchAsync(loaiDT, strFeatures);
                        resultOffset = ((i + 1) * maxRecordPerPage) + 1;
                    }
                } else
                {
                    _logger.LogInformation($"Loại đối tượng {loaiDT}. Không có bản ghi nào được cập nhật.");
                }
            } catch(Exception ex)
            {
                //Chỉ ghi log lỗi, không ném ngoại lệ.
                _logger.LogError(ex.Message, ex);
            }
        }

    }
}

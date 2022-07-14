using eNPT_DongBoDuLieu.Models;
using eNPT_DongBoDuLieu.Models.Services;
using eNPT_DongBoDuLieu.Services.DataBases;
using eNPT_DongBoDuLieu.Services.Datas;
using eNPT_DongBoDuLieu.Services.Portals;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    _logger.LogInformation($"Bắt đầu đồng bộ dữ liệu: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}.");

                    //Get Token
                    var resultGenToken = await _portalServices.GenerateTokenAsync();

                    #region "Cập nhật dữ liệu"
                    //Read Data LastEditDate From File
                    var lastEditDate = await _dataServices.ReadDataAsync();
                    //Cập nhật dữ liệu Đường dây điện được chỉnh sửa
                    await RunAsync(resultGenToken.token, ELoaiDT.DDA, lastEditDate);
                    //Cập nhật dữ liệu Trạm biến áp được chỉnh sửa
                    await RunAsync(resultGenToken.token, ELoaiDT.TBA, lastEditDate);
                    //Cập nhật dữ liệu Cột điện được chỉnh sửa
                    await RunAsync(resultGenToken.token, ELoaiDT.COT, lastEditDate);

                    //Write Data LastEditDate To File
                    await _dataServices.WriteDataAsync(lastEditDate);
                    #endregion

                    #region "Xóa dữ liệu"
                    //Xóa dữ liệu Đường dây điện không còn tồn tại
                    await RunDeleteAsync(resultGenToken.token, ELoaiDT.DDA);
                    //Xóa dữ liệu Trạm biến áp không còn tồn tại
                    await RunDeleteAsync(resultGenToken.token, ELoaiDT.TBA);
                    //Xóa dữ liệu Cột điện không còn tồn tại
                    await RunDeleteAsync(resultGenToken.token, ELoaiDT.COT);
                    #endregion

                    _logger.LogInformation($"Hoàn thành đồng bộ dữ liệu: {DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")}.");
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
        /// <param name="loaiDT">Loại đối tượng</param>
        /// <param name="lastEditDate">Thời gian</param>
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
                }
            } catch(Exception ex)
            {
                //Chỉ ghi log lỗi, không ném ngoại lệ.
                _logger.LogError(ex.Message, ex);
            }
        }

        /// <summary>
        /// Hàm thực hiện xóa dữ liệu không tồn tại trên Feature
        /// </summary>
        /// <param name="token"></param>
        /// <param name="loaiDT">Loại đối tượng</param>
        /// <returns></returns>
        private async Task RunDeleteAsync(string token, ELoaiDT loaiDT)
        {
            try
            {
                _logger.LogInformation($"Thực hiện xóa dữ liệu đối tượng {loaiDT}...");
                var countRecord = await _dataBaseServices.GetCountByLoaiDTFullTextSearchAsync(loaiDT);
                var maxRecordPerPage = _appSettings.MaxRecordPerPage;
                if (countRecord != 0)
                {
                    _logger.LogInformation($"Loại đối tượng {loaiDT}. Có {countRecord} bản ghi cần được kiểm tra.");
                    var nPage = countRecord / maxRecordPerPage;
                    if (countRecord % _appSettings.MaxRecordPerPage != 0)
                    {
                        nPage++;
                    }
                    for (var i = 0; i < nPage; i++)
                    {
                        _logger.LogInformation($"Đang kiểm tra dữ liệu loại đối tượng {loaiDT} trang {i + 1} trên tổng số {nPage} trang...");
                        //Danh sách OID cần kiểm tra xem còn tồn tại trên Feature hay không
                        var objectIds = await _dataBaseServices.GetObjectIDFullTextSearchAsyncs(loaiDT, (i * maxRecordPerPage) + 1, ((i + 1) * maxRecordPerPage));
                        if(objectIds.Count != 0)
                        {
                            //Danh sách OID còn tồn tại
                            var objectIdFeatures = await _portalServices.GetObjectIDByOIDAsyncs(token, loaiDT, objectIds);
                            var dicObjectIDFeatures = objectIdFeatures
                                .Select(it => new { Key = $"{it}", Value = it })
                                .ToDictionary(it => it.Key, it => it.Value);
                            //Danh sách OBJECTID sẽ không còn tồn tại trên Feature, cần xóa trong EN_FULLTEXTSEARCH, EN_COTDIEN, EN_DUONGDAY, EN_TRAMBIENAP
                            var objectIDDeletes = new List<string>();
                            foreach (var oId in objectIds)
                            {
                                if (!dicObjectIDFeatures.ContainsKey(oId))
                                {
                                    objectIDDeletes.Add(oId);
                                }
                            }
                            if(objectIDDeletes.Count != 0)
                            {
                                //Xóa dữ liệu
                                await _dataBaseServices.DeleteFullTextSearchCotTramDuongDayAsync(loaiDT, objectIDDeletes);
                            }
                        }
                    }
                }
            } catch(Exception ex)
            {
                //Chỉ ghi log lỗi, không ném ngoại lệ.
                _logger.LogError(ex.Message, ex);
            }
        }
    }
}

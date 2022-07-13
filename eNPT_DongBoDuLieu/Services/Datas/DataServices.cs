using eNPT_DongBoDuLieu.Models;
using eNPT_DongBoDuLieu.Models.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.Datas
{
    public class DataServices : IDataServices
    {
        private readonly ILogger<DataServices> _logger;
        private readonly AppSettings _appSettings;
        //Đối tượng SemaphoreSlim được sử dụng để kiểm soát quyền truy cập vào một tài nguyên
        private readonly SemaphoreSlim _semaphoregate;

        public DataServices(IOptions<AppSettings> appSettings, ILogger<DataServices> logger)
        {
            _semaphoregate = new SemaphoreSlim(1);
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        public async Task WriteDataAsync(LastEditDate lastEditDate)
        {
            try
            {
                await _semaphoregate.WaitAsync();
                //Gán dữ liệu trường 'NgayGio' = 'NgayGioHienTai'.
                lastEditDate.NgayGio = lastEditDate.NgayGioHienTai;
                var allText = JsonConvert.SerializeObject(lastEditDate, Formatting.Indented);
                await File.WriteAllTextAsync(_appSettings.PathFileLastEditDates, allText);
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            } finally
            {
                _semaphoregate.Release();
            }
        }

        public async Task<LastEditDate> ReadDataAsync()
        {
            LastEditDate ret = null;
            try
            {
                await _semaphoregate.WaitAsync();
                var allText = await File.ReadAllTextAsync(_appSettings.PathFileLastEditDates);
                ret = JsonConvert.DeserializeObject<LastEditDate>(allText);
                //Khởi tạo trường 'NgayGioHienTai' (ngày giờ hiện tại).
                ret.NgayGioHienTai = DateTime.Now;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            finally
            {
                _semaphoregate.Release();
            }
            return ret;
        }
    }
}

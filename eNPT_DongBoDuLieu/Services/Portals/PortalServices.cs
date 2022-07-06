using eNPT_DongBoDuLieu.Models;
using eNPT_DongBoDuLieu.Models.GISSystemInfor;
using eNPT_DongBoDuLieu.Models.Portals;
using eNPT_DongBoDuLieu.Models.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.Portals
{
    public class PortalServices : IPortalServices
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GISSystemInfor _gISSystemInfor;
        private readonly ILogger<PortalServices> _logger;
        private readonly AppSettings _appSettings;

        public PortalServices(
            ILogger<PortalServices> logger,
            IOptions<GISSystemInfor> gISSystemInfor,
            IOptions<AppSettings> appSettings,
            IHttpClientFactory httpClientFactory)
        {
            _gISSystemInfor = gISSystemInfor.Value;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _appSettings = appSettings.Value;
        }

        public async Task<ResultGenerateToken> GenerateTokenAsync()
        {
            ResultGenerateToken ret = null;
            try
            {
                var formdata = new MultipartFormDataContent();
                formdata.Add(new StringContent(_gISSystemInfor.UserName), "username");
                formdata.Add(new StringContent(_gISSystemInfor.Password), "password");
                formdata.Add(new StringContent("referer"), "client");
                formdata.Add(new StringContent(_gISSystemInfor.ArcGISPortalDirectory.RootPath), "referer");
                formdata.Add(new StringContent($"{_appSettings.ExpirationToken}"), "expiration");
                formdata.Add(new StringContent("json"), "f");
                var client = _httpClientFactory.CreateClient("HttpClientFactory");
                var response = await client.PostAsync(_gISSystemInfor.ArcGISPortalDirectory.GenerateToken, formdata);
                var resultStr = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonObj = JObject.Parse(resultStr);
                    if (jsonObj["error"] != null)
                    {
                        //Lỗi
                        _logger.LogError($"Lỗi query trả về: {resultStr}");
                    }
                    else
                    {
                        //Thành công
                        ret = JsonConvert.DeserializeObject<ResultGenerateToken>(resultStr);
                    }
                }
                else
                {
                    _logger.LogError($"Lỗi xảy ra khi gọi tới: {_gISSystemInfor.ArcGISPortalDirectory.GenerateToken}");
                }
                if(ret == null)
                {
                    throw new Exception("Thực hiện GenerateToken thất bại.");
                }
            } catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public async Task<int> GetCountByLastEditTimeAsync(string token, ELoaiDT loaiDT, LastEditDate lastEditDate)
        {
            int ret = -1;
            try
            {
                var linkFeatureService = GetLinkFeatureService(loaiDT);

                var formdata = new MultipartFormDataContent();
                var conditionQuery = $"{_appSettings.FieldName} >= TIMESTAMP '{lastEditDate.NgayGio.ToString("yyyy-MM-dd HH:mm:ss")}' AND {_appSettings.FieldName} <= TIMESTAMP '{lastEditDate.NgayGioHienTai.ToString("yyyy-MM-dd HH:mm:ss")}'";
                formdata.Add(new StringContent(conditionQuery), "where");
                formdata.Add(new StringContent("true"), "returnCountOnly");
                formdata.Add(new StringContent("json"), "f");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{linkFeatureService}/query");
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Content = formdata;

                var client = _httpClientFactory.CreateClient("HttpClientFactory");

                var response = await client.SendAsync(request);
                var resultStr = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    var jsonObj = JObject.Parse(resultStr);
                    if(jsonObj["error"] != null)
                    {
                        //Lỗi
                        _logger.LogError($"Lỗi query trả về: {resultStr}");
                    }
                    else
                    {
                        //Thành công
                        ret = (int)jsonObj["count"];
                    }
                } else
                {
                    _logger.LogError($"Gọi tới FeatureService '{linkFeatureService}' không thành công.");
                }
                if(ret == -1)
                {
                    throw new Exception("Thực hiện GetCountByLastEditTime thất bại.");
                }
            } catch(Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public async Task<string> GetFeatureAsyncs(string token, ELoaiDT loaiDT, LastEditDate lastEditDate, int resultOffset, int resultRecordCount)
        {
            var ret = "";
            try
            {
                var linkFeatureService = GetLinkFeatureService(loaiDT);

                var formdata = new MultipartFormDataContent();
                var conditionQuery = $"{_appSettings.FieldName} >= TIMESTAMP '{lastEditDate.NgayGio.ToString("yyyy-MM-dd HH:mm:ss")}' AND {_appSettings.FieldName} <= TIMESTAMP '{lastEditDate.NgayGioHienTai.ToString("yyyy-MM-dd HH:mm:ss")}'";
                formdata.Add(new StringContent(conditionQuery), "where");
                formdata.Add(new StringContent($"{resultOffset}"), "resultOffset");
                formdata.Add(new StringContent($"{resultRecordCount}"), "resultRecordCount");
                formdata.Add(new StringContent("*"), "outFields");
                //formdata.Add(new StringContent($"{_appSettings.FieldName} DESC"), "orderByFields");
                formdata.Add(new StringContent("json"), "f");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{linkFeatureService}/query");
                request.Headers.Add("Authorization", $"Bearer {token}");
                request.Content = formdata;

                var client = _httpClientFactory.CreateClient("HttpClientFactory");

                var response = await client.SendAsync(request);
                var resultStr = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var jsonObj = JObject.Parse(resultStr);
                    if (jsonObj["error"] != null)
                    {
                        //Lỗi
                        _logger.LogError($"Lỗi query trả về: {resultStr}");
                    }
                    else
                    {
                        //Thành công
                        ret = jsonObj["features"].ToString();
                    }
                }
                else
                {
                    _logger.LogError($"Gọi tới FeatureService '{linkFeatureService}' không thành công.");
                }
                if (ret == "")
                {
                    throw new Exception("Thực hiện GetFeatures thất bại.");
                }
            } catch(Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        private string GetLinkFeatureService(ELoaiDT loaiDT)
        {
            var ret = "";
            try
            {
                switch (loaiDT)
                {
                    case ELoaiDT.DDA:
                        ret = _gISSystemInfor.ArcGISRESTServicesDirectory.DuongDayFeatureServer;
                        break;
                    case ELoaiDT.TBA:
                        ret = _gISSystemInfor.ArcGISRESTServicesDirectory.TramBienApFeatureServer;
                        break;
                    case ELoaiDT.COT:
                        ret = _gISSystemInfor.ArcGISRESTServicesDirectory.CotFeatureServer;
                        break;
                }
                if(ret == "")
                {
                    throw new Exception($"Dữ liệu Link FeatureService rỗng. Với 'Loại Đối Tượng' = {loaiDT}.");
                }
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw ex;
            }
            return ret;
        }

    }
}

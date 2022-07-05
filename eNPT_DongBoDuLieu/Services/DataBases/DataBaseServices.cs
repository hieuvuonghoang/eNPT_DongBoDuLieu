using eNPT_DongBoDuLieu.Models.DataBases.EVNNPT;
using eNPT_DongBoDuLieu.Models.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.DataBases
{
    public class DataBaseServices : IDataBaseServices
    {
        private readonly ILogger<DataBaseServices> _logger;
        private readonly ModelContext _context;

        public DataBaseServices(
            ILogger<DataBaseServices> logger, 
            ModelContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task RemoveAndInsertFullTextSearch(ELoaiDT loaiDT, string strFeatures)
        {
            try
            {
                var features = JArray.Parse(strFeatures);
                var objIds = new List<int>();
                foreach(var feature in features)
                {
                    objIds.Add((int)feature["attributes"]["OBJECTID"]);
                    var str = feature["attributes"].ToString();
                    var cotDien = JsonConvert.DeserializeObject<EN_COTDIEN>(str);
                    var strCotDien = JsonConvert.SerializeObject(cotDien, Formatting.Indented);
                }
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            throw new NotImplementedException();
        }
    }
}

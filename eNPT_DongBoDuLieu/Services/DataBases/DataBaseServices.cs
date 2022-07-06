using eNPT_DongBoDuLieu.Models.DataBases.EVNNPT;
using eNPT_DongBoDuLieu.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task DeleteAddInsertFeaturesAsync(ELoaiDT loaiDT, List<EN_FULLTEXTSEARCH> fullTextSearchs)
        {
            try
            {
                var pKTables = "";
                var sQLDelete = "";
                var tableName = "";
                switch (loaiDT)
                {
                    case ELoaiDT.COT:
                        tableName = "EN_COTDIEN";
                        pKTables = string.Join(",", fullTextSearchs.Select(it => it.CotDien.MA_COT).ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE MA_COT IN ({pKTables})";
                        break;
                    case ELoaiDT.DDA:
                        tableName = "EN_DUONGDAY";
                        pKTables = string.Join(",", fullTextSearchs.Select(it => it.DuongDay.MADUONGDAY).ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE MADUONGDAY IN ({pKTables})";
                        break;
                    case ELoaiDT.TBA:
                        tableName = "EN_TRAMBIENAP";
                        pKTables = string.Join(",", fullTextSearchs.Select(it => it.TramBienAp.MATRAM).ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE MATRAM IN ({pKTables})";
                        break;
                }
                //Begin transaction
                var transaction = await _context.Database.BeginTransactionAsync();
                //Remove
                var numRowDelete = await _context.Database.ExecuteSqlRawAsync(sQLDelete);
                _logger.LogInformation($"Số bản ghi bị xóa trong bảng {tableName}: {numRowDelete}");
                //Insert
                switch (loaiDT)
                {
                    case ELoaiDT.COT:
                        await _context.EN_COTDIENs.AddRangeAsync(fullTextSearchs.Select(it => it.CotDien).ToList());
                        break;
                    case ELoaiDT.DDA:
                        await _context.EN_DUONGDAYs.AddRangeAsync(fullTextSearchs.Select(it => it.DuongDay).ToList());
                        break;
                    case ELoaiDT.TBA:
                        await _context.EN_TRAMBIENAPs.AddRangeAsync(fullTextSearchs.Select(it => it.TramBienAp).ToList());
                        break;
                }
                var numRowAdd = await _context.SaveChangesAsync();
                _logger.LogInformation($"Số bản ghi được thêm vào bảng {tableName}: {numRowAdd}");
                //Commit
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public async Task DeleteAndInsertFullTextSearchAsync(ELoaiDT loaiDT, string strFeatures)
        {
            try
            {
                var features = JArray.Parse(strFeatures);
                var objIds = new List<int>();
                var fullTextSearchs = new List<EN_FULLTEXTSEARCH>();
                foreach (var feature in features)
                {
                    objIds.Add((int)feature["attributes"]["OBJECTID"]);
                    fullTextSearchs.Add(MapStrFeatureToFullTextSearch(loaiDT, feature["attributes"].ToString(), feature.ToString()));
                }
                var strObjIds = string.Join(",", objIds.Select(it => it).ToList());
                //Begin transaction
                var transaction = await _context.Database.BeginTransactionAsync();
                //Remove
                var sQLDelete = $"DELETE FROM EN_FULLTEXTSEARCH WHERE LOAIDT = '{loaiDT}' AND OBJECTID IN ({strObjIds})";
                var numRowDelete = await _context.Database.ExecuteSqlRawAsync(sQLDelete);
                _logger.LogInformation($"Số bản ghi bị xóa trong bảng EN_FULLTEXTSEARCH: {numRowDelete}");
                //Insert
                await _context.EN_FULLTEXTSEARCHes.AddRangeAsync(fullTextSearchs);
                var numRowAdd = await _context.SaveChangesAsync();
                _logger.LogInformation($"Số bản ghi được thêm vào bảng EN_FULLTEXTSEARCH: {numRowAdd}");
                //Commit
                await transaction.CommitAsync();
                //
                await DeleteAddInsertFeaturesAsync(loaiDT, fullTextSearchs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        /// <summary>
        /// Ánh xạ dữ liệu feature sang dữ liệu FULLTEXTSEARCH
        /// </summary>
        /// <param name="loaiDT">Loại đối tượng</param>
        /// <param name="featureAttributes">Dữ liệu attributes của feature</param>
        /// <param name="feature">Dữ liệu full của feature (attributes và geometry)</param>
        /// <returns></returns>
        private EN_FULLTEXTSEARCH MapStrFeatureToFullTextSearch(ELoaiDT loaiDT, string featureAttributes, string feature)
        {
            EN_FULLTEXTSEARCH ret = new EN_FULLTEXTSEARCH();
            try
            {
                switch (loaiDT)
                {
                    case ELoaiDT.COT:
                        var cotDien = JsonConvert.DeserializeObject<EN_COTDIEN>(featureAttributes);
                        ret.FTSID = Guid.NewGuid().ToString();
                        ret.LOAIDT = "COT";
                        ret.DATA = feature;
                        ret.OBJECTID = cotDien.OBJECTID.ToString();
                        ret.MADT = cotDien.MA_COT;
                        ret.TENDT = cotDien.TEN_COT;
                        ret.MADVQL = cotDien.MADVQL;
                        ret.CotDien = cotDien;
                        break;
                    case ELoaiDT.TBA:
                        var tramBienAp = JsonConvert.DeserializeObject<EN_TRAMBIENAP>(featureAttributes);
                        ret.FTSID = Guid.NewGuid().ToString();
                        ret.LOAIDT = "TBA";
                        ret.DATA = feature;
                        ret.OBJECTID = tramBienAp.OBJECTID.ToString();
                        ret.MADT = tramBienAp.MATRAM;
                        ret.TENDT = tramBienAp.TEN_TRAM;
                        ret.MADVQL = tramBienAp.MADVQL;
                        ret.TramBienAp = tramBienAp;
                        break;
                    case ELoaiDT.DDA:
                        var duongDayDien = JsonConvert.DeserializeObject<EN_DUONGDAY>(featureAttributes);
                        ret.FTSID = Guid.NewGuid().ToString();
                        ret.LOAIDT = "DDA";
                        ret.DATA = feature;
                        ret.OBJECTID = duongDayDien.OBJECTID.ToString();
                        ret.MADT = duongDayDien.MADUONGDAY;
                        ret.TENDT = duongDayDien.TENDUONGDAY;
                        ret.MADVQL = duongDayDien.MADVQL;
                        ret.DuongDay = duongDayDien;
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return ret;
        }

    }
}

using eNPT_DongBoDuLieu.Models.DataBases.EVNNPT;
using eNPT_DongBoDuLieu.Models.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.DataBases
{
    public class DataBaseServices : IDataBaseServices
    {
        private readonly ILogger<DataBaseServices> _logger;
        private readonly ModelContext _context;
        private readonly string _insertCotDien;

        public DataBaseServices(
            ILogger<DataBaseServices> logger,
            ModelContext context)
        {
            _logger = logger;
            _context = context;
            _insertCotDien = @"INSERT INTO EVNNPT.EN_COTDIEN (
                               MA_COT, LONG_, LAT, 
                               CONGDUNGCOT, CHIEUCAO, THUTU_PHA, 
                               SOMACH_DD, TRONGLUONG, SOHUU, 
                               SOMACH_DCS, LOAI_MC, LOAI_BLNM, 
                               KEMONG, TINH, HUYEN, 
                               XA, LOAI_TD, MADVQL, 
                               MAKVHC, CAPDA, TEN_TTD, 
                               MAVITRI, TENVITRI, MATIEPDIA, 
                               TEN_COT, MA_DCS, MACT, 
                               TENCT, MATTDKV) 
                               VALUES (:MA_COT, :LONG_, :LAT, 
                                       :CONGDUNGCOT, :CHIEUCAO, :THUTU_PHA,
                                       :SOMACH_DD, :TRONGLUONG, :SOHUU,
                                       :SOMACH_DCS, :LOAI_MC, :LOAI_BLNM,
                                       :KEMONG, :TINH, :HUYEN, 
                                       :XA, :LOAI_TD, :MADVQL, 
                                       :MAKVHC, :CAPDA, :TEN_TTD, 
                                       :MAVITRI, :TENVITRI, :MATIEPDIA,
                                       :TEN_COT, :MA_DCS, :MACT, 
                                       :TENCT, :MATTDKV)";
        }

        public async Task DeleteAddInsertFeaturesAsync(ELoaiDT loaiDT, List<EN_FULLTEXTSEARCH> fullTextSearchs)
        {
            try
            {
                var pKTables = "";
                var sQLDelete = "";
                var tableName = "";
                var dicPK = new Dictionary<string, EN_FULLTEXTSEARCH>();
                switch (loaiDT)
                {
                    case ELoaiDT.COT:
                        tableName = "EN_COTDIEN";
                        foreach (var fullTextSearch in fullTextSearchs)
                        {
                            if (!dicPK.ContainsKey(fullTextSearch.CotDien.MA_COT))
                            {
                                dicPK.Add(fullTextSearch.CotDien.MA_COT, fullTextSearch);
                            }
                        }
                        pKTables = string.Join(",", dicPK.Keys.Select(it => $"'{it}'").ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE MA_COT IN ({pKTables})";
                        break;
                    case ELoaiDT.DDA:
                        tableName = "EN_DUONGDAY";
                        foreach (var fullTextSearch in fullTextSearchs)
                        {
                            if (!dicPK.ContainsKey(fullTextSearch.DuongDay.MADUONGDAY))
                            {
                                dicPK.Add(fullTextSearch.DuongDay.MADUONGDAY, fullTextSearch);
                            }
                        }
                        pKTables = string.Join(",", fullTextSearchs.Select(it => $"'{it.DuongDay.MADUONGDAY}'").ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE MADUONGDAY IN ({pKTables})";
                        break;
                    case ELoaiDT.TBA:
                        tableName = "EN_TRAMBIENAP";
                        foreach (var fullTextSearch in fullTextSearchs)
                        {
                            if (!dicPK.ContainsKey(fullTextSearch.TramBienAp.MATRAM))
                            {
                                dicPK.Add(fullTextSearch.TramBienAp.MATRAM, fullTextSearch);
                            }
                        }
                        pKTables = string.Join(",", fullTextSearchs.Select(it => $"'{it.TramBienAp.MATRAM}'").ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE MATRAM IN ({pKTables})";
                        break;
                }
                _logger.LogInformation(pKTables);
                var numRowDelete = 0;
                var numRowAdd = 0;
                if (!string.IsNullOrEmpty(sQLDelete))
                {
                    //Delete
                    using (var command = _context.Database.GetDbConnection().CreateCommand())
                    {

                        command.CommandType = System.Data.CommandType.Text;
                        command.CommandText = sQLDelete;

                        _logger.LogInformation(command.CommandText);
                        await _context.Database.OpenConnectionAsync();

                        numRowDelete = await command.ExecuteNonQueryAsync();

                        _logger.LogInformation($"Số bản ghi bị xóa trong bảng {tableName}: {numRowDelete}");
                    }
                }
                switch (loaiDT)
                {
                    case ELoaiDT.COT:
                        //Insert
                        using (var command = _context.Database.GetDbConnection().CreateCommand())
                        {

                            command.CommandType = System.Data.CommandType.Text;
                            command.CommandText = _insertCotDien;

                            #region "DbParameter"
                            DbParameter maCot = command.CreateParameter();
                            maCot.DbType = DbType.String;
                            maCot.ParameterName = "MA_COT";
                            command.Parameters.Add(maCot);

                            DbParameter long_ = command.CreateParameter();
                            long_.DbType = DbType.Decimal;
                            long_.ParameterName = "LONG_";
                            command.Parameters.Add(long_);

                            DbParameter lat = command.CreateParameter();
                            lat.DbType = DbType.Decimal;
                            lat.ParameterName = "LAT";
                            command.Parameters.Add(lat);

                            DbParameter congDungCot = command.CreateParameter();
                            congDungCot.DbType = DbType.String;
                            congDungCot.ParameterName = "CONGDUNGCOT";
                            command.Parameters.Add(congDungCot);

                            DbParameter chieuCao = command.CreateParameter();
                            chieuCao.DbType = DbType.Decimal;
                            chieuCao.ParameterName = "CHIEUCAO";
                            command.Parameters.Add(chieuCao);

                            DbParameter thuTuPha = command.CreateParameter();
                            thuTuPha.DbType = DbType.String;
                            thuTuPha.ParameterName = "THUTU_PHA";
                            command.Parameters.Add(thuTuPha);

                            DbParameter soMachDD = command.CreateParameter();
                            soMachDD.DbType = DbType.Decimal;
                            soMachDD.ParameterName = "SOMACH_DD";
                            command.Parameters.Add(soMachDD);

                            DbParameter trongLuong = command.CreateParameter();
                            trongLuong.DbType = DbType.Decimal;
                            trongLuong.ParameterName = "TRONGLUONG";
                            command.Parameters.Add(trongLuong);

                            DbParameter soHuu = command.CreateParameter();
                            soHuu.DbType = DbType.String;
                            soHuu.ParameterName = "SOHUU";
                            command.Parameters.Add(soHuu);

                            DbParameter soMachDCS = command.CreateParameter();
                            soMachDCS.DbType = DbType.Decimal;
                            soMachDCS.ParameterName = "SOMACH_DCS";
                            command.Parameters.Add(soMachDCS);

                            DbParameter loaiMC = command.CreateParameter();
                            loaiMC.DbType = DbType.String;
                            loaiMC.ParameterName = "LOAI_MC";
                            command.Parameters.Add(loaiMC);

                            DbParameter loaiBLNM = command.CreateParameter();
                            loaiBLNM.DbType = DbType.String;
                            loaiBLNM.ParameterName = "LOAI_BLNM";
                            command.Parameters.Add(loaiBLNM);

                            DbParameter keMong = command.CreateParameter();
                            keMong.DbType = DbType.String;
                            keMong.ParameterName = "KEMONG";
                            command.Parameters.Add(keMong);

                            DbParameter tinh = command.CreateParameter();
                            tinh.DbType = DbType.String;
                            tinh.ParameterName = "TINH";
                            command.Parameters.Add(tinh);

                            DbParameter huyen = command.CreateParameter();
                            huyen.DbType = DbType.String;
                            huyen.ParameterName = "HUYEN";
                            command.Parameters.Add(huyen);

                            DbParameter xa = command.CreateParameter();
                            xa.DbType = DbType.String;
                            xa.ParameterName = "XA";
                            command.Parameters.Add(xa);

                            DbParameter loaiTD = command.CreateParameter();
                            loaiTD.DbType = DbType.String;
                            loaiTD.ParameterName = "LOAI_TD";
                            command.Parameters.Add(loaiTD);

                            DbParameter maDVQL = command.CreateParameter();
                            maDVQL.DbType = DbType.String;
                            maDVQL.ParameterName = "MADVQL";
                            command.Parameters.Add(maDVQL);

                            DbParameter maKVHC = command.CreateParameter();
                            maKVHC.DbType = DbType.String;
                            maKVHC.ParameterName = "MAKVHC";
                            command.Parameters.Add(maKVHC);

                            DbParameter capDA = command.CreateParameter();
                            capDA.DbType = DbType.String;
                            capDA.ParameterName = "CAPDA";
                            command.Parameters.Add(capDA);

                            DbParameter tenTTD = command.CreateParameter();
                            tenTTD.DbType = DbType.String;
                            tenTTD.ParameterName = "TEN_TTD";
                            command.Parameters.Add(tenTTD);

                            DbParameter maViTri = command.CreateParameter();
                            maViTri.DbType = DbType.String;
                            maViTri.ParameterName = "MAVITRI";
                            command.Parameters.Add(maViTri);

                            DbParameter tenViTri = command.CreateParameter();
                            tenViTri.DbType = DbType.String;
                            tenViTri.ParameterName = "TENVITRI";
                            command.Parameters.Add(tenViTri);

                            DbParameter maTiepDia = command.CreateParameter();
                            maTiepDia.DbType = DbType.String;
                            maTiepDia.ParameterName = "MATIEPDIA";
                            command.Parameters.Add(maTiepDia);

                            DbParameter tenCot = command.CreateParameter();
                            tenCot.DbType = DbType.String;
                            tenCot.ParameterName = "TEN_COT";
                            command.Parameters.Add(tenCot);

                            DbParameter maDCS = command.CreateParameter();
                            maDCS.DbType = DbType.String;
                            maDCS.ParameterName = "MA_DCS";
                            command.Parameters.Add(maDCS);

                            DbParameter maCT = command.CreateParameter();
                            maCT.DbType = DbType.String;
                            maCT.ParameterName = "MACT";
                            command.Parameters.Add(maCT);

                            DbParameter tenCT = command.CreateParameter();
                            tenCT.DbType = DbType.String;
                            tenCT.ParameterName = "TENCT";
                            command.Parameters.Add(tenCT);

                            DbParameter maTTDKV = command.CreateParameter();
                            maTTDKV.DbType = DbType.String;
                            maTTDKV.ParameterName = "MATTDKV";
                            command.Parameters.Add(maTTDKV);

                            #endregion

                            _logger.LogInformation(command.CommandText);
                            await _context.Database.OpenConnectionAsync();

                            foreach(var cotDien in dicPK.Values.Select(it => it.CotDien).ToList())
                            {
                                maCot.Value = cotDien.MA_COT;
                                long_.Value = cotDien.LONG_;
                                lat.Value = cotDien.LAT;
                                congDungCot.Value = cotDien.CONGDUNGCOT;
                                chieuCao.Value = cotDien.CHIEUCAO;
                                thuTuPha.Value = cotDien.THUTU_PHA;
                                soMachDD.Value = cotDien.SOMACH_DD;
                                trongLuong.Value = cotDien.TRONGLUONG;
                                soHuu.Value = cotDien.SOHUU;
                                soMachDCS.Value = cotDien.SOMACH_DCS;
                                loaiMC.Value = cotDien.LOAI_MC;
                                loaiBLNM.Value = cotDien.LOAI_BLNM;
                                keMong.Value = cotDien.KEMONG;
                                tinh.Value = cotDien.TINH;
                                huyen.Value = cotDien.HUYEN;
                                xa.Value = cotDien.XA;
                                loaiTD.Value = cotDien.LOAI_TD;
                                maDVQL.Value = cotDien.MADVQL;
                                maKVHC.Value = cotDien.MAKVHC;
                                capDA.Value = cotDien.CAPDA;
                                tenTTD.Value = cotDien.TEN_TTD;
                                maViTri.Value = cotDien.MAVITRI;
                                tenViTri.Value = cotDien.TENVITRI;
                                maTiepDia.Value = cotDien.MATIEPDIA;
                                tenCot.Value = cotDien.TEN_COT;
                                maDCS.Value = cotDien.MA_DCS;
                                maCT.Value = cotDien.MACT;
                                tenCT.Value = cotDien.TENCT;
                                maTTDKV.Value = cotDien.MATTDKV;
                                numRowAdd += await command.ExecuteNonQueryAsync();
                            }
                        }
                        break;
                    case ELoaiDT.DDA:
                        await _context.EN_DUONGDAYs.AddRangeAsync(fullTextSearchs.Select(it => it.DuongDay).ToList());
                        break;
                    case ELoaiDT.TBA:
                        //Insert
                        using (var command = _context.Database.GetDbConnection().CreateCommand())
                        {

                            command.CommandType = System.Data.CommandType.Text;
                            command.CommandText = _insertCotDien;

                            #region "DbParameter"
                            DbParameter maCot = command.CreateParameter();
                            maCot.DbType = DbType.String;
                            maCot.ParameterName = "MA_COT";
                            command.Parameters.Add(maCot);

                            DbParameter long_ = command.CreateParameter();
                            long_.DbType = DbType.Decimal;
                            long_.ParameterName = "LONG_";
                            command.Parameters.Add(long_);

                            DbParameter lat = command.CreateParameter();
                            lat.DbType = DbType.Decimal;
                            lat.ParameterName = "LAT";
                            command.Parameters.Add(lat);

                            DbParameter congDungCot = command.CreateParameter();
                            congDungCot.DbType = DbType.String;
                            congDungCot.ParameterName = "CONGDUNGCOT";
                            command.Parameters.Add(congDungCot);

                            DbParameter chieuCao = command.CreateParameter();
                            chieuCao.DbType = DbType.Decimal;
                            chieuCao.ParameterName = "CHIEUCAO";
                            command.Parameters.Add(chieuCao);

                            DbParameter thuTuPha = command.CreateParameter();
                            thuTuPha.DbType = DbType.String;
                            thuTuPha.ParameterName = "THUTU_PHA";
                            command.Parameters.Add(thuTuPha);

                            DbParameter soMachDD = command.CreateParameter();
                            soMachDD.DbType = DbType.Decimal;
                            soMachDD.ParameterName = "SOMACH_DD";
                            command.Parameters.Add(soMachDD);

                            DbParameter trongLuong = command.CreateParameter();
                            trongLuong.DbType = DbType.Decimal;
                            trongLuong.ParameterName = "TRONGLUONG";
                            command.Parameters.Add(trongLuong);

                            DbParameter soHuu = command.CreateParameter();
                            soHuu.DbType = DbType.String;
                            soHuu.ParameterName = "SOHUU";
                            command.Parameters.Add(soHuu);

                            DbParameter soMachDCS = command.CreateParameter();
                            soMachDCS.DbType = DbType.Decimal;
                            soMachDCS.ParameterName = "SOMACH_DCS";
                            command.Parameters.Add(soMachDCS);

                            DbParameter loaiMC = command.CreateParameter();
                            loaiMC.DbType = DbType.String;
                            loaiMC.ParameterName = "LOAI_MC";
                            command.Parameters.Add(loaiMC);

                            DbParameter loaiBLNM = command.CreateParameter();
                            loaiBLNM.DbType = DbType.String;
                            loaiBLNM.ParameterName = "LOAI_BLNM";
                            command.Parameters.Add(loaiBLNM);

                            DbParameter keMong = command.CreateParameter();
                            keMong.DbType = DbType.String;
                            keMong.ParameterName = "KEMONG";
                            command.Parameters.Add(keMong);

                            DbParameter tinh = command.CreateParameter();
                            tinh.DbType = DbType.String;
                            tinh.ParameterName = "TINH";
                            command.Parameters.Add(tinh);

                            DbParameter huyen = command.CreateParameter();
                            huyen.DbType = DbType.String;
                            huyen.ParameterName = "HUYEN";
                            command.Parameters.Add(huyen);

                            DbParameter xa = command.CreateParameter();
                            xa.DbType = DbType.String;
                            xa.ParameterName = "XA";
                            command.Parameters.Add(xa);

                            DbParameter loaiTD = command.CreateParameter();
                            loaiTD.DbType = DbType.String;
                            loaiTD.ParameterName = "LOAI_TD";
                            command.Parameters.Add(loaiTD);

                            DbParameter maDVQL = command.CreateParameter();
                            maDVQL.DbType = DbType.String;
                            maDVQL.ParameterName = "MADVQL";
                            command.Parameters.Add(maDVQL);

                            DbParameter maKVHC = command.CreateParameter();
                            maKVHC.DbType = DbType.String;
                            maKVHC.ParameterName = "MAKVHC";
                            command.Parameters.Add(maKVHC);

                            DbParameter capDA = command.CreateParameter();
                            capDA.DbType = DbType.String;
                            capDA.ParameterName = "CAPDA";
                            command.Parameters.Add(capDA);

                            DbParameter tenTTD = command.CreateParameter();
                            tenTTD.DbType = DbType.String;
                            tenTTD.ParameterName = "TEN_TTD";
                            command.Parameters.Add(tenTTD);

                            DbParameter maViTri = command.CreateParameter();
                            maViTri.DbType = DbType.String;
                            maViTri.ParameterName = "MAVITRI";
                            command.Parameters.Add(maViTri);

                            DbParameter tenViTri = command.CreateParameter();
                            tenViTri.DbType = DbType.String;
                            tenViTri.ParameterName = "TENVITRI";
                            command.Parameters.Add(tenViTri);

                            DbParameter maTiepDia = command.CreateParameter();
                            maTiepDia.DbType = DbType.String;
                            maTiepDia.ParameterName = "MATIEPDIA";
                            command.Parameters.Add(maTiepDia);

                            DbParameter tenCot = command.CreateParameter();
                            tenCot.DbType = DbType.String;
                            tenCot.ParameterName = "TEN_COT";
                            command.Parameters.Add(tenCot);

                            DbParameter maDCS = command.CreateParameter();
                            maDCS.DbType = DbType.String;
                            maDCS.ParameterName = "MA_DCS";
                            command.Parameters.Add(maDCS);

                            DbParameter maCT = command.CreateParameter();
                            maCT.DbType = DbType.String;
                            maCT.ParameterName = "MACT";
                            command.Parameters.Add(maCT);

                            DbParameter tenCT = command.CreateParameter();
                            tenCT.DbType = DbType.String;
                            tenCT.ParameterName = "TENCT";
                            command.Parameters.Add(tenCT);

                            DbParameter maTTDKV = command.CreateParameter();
                            maTTDKV.DbType = DbType.String;
                            maTTDKV.ParameterName = "MATTDKV";
                            command.Parameters.Add(maTTDKV);

                            #endregion

                            _logger.LogInformation(command.CommandText);
                            await _context.Database.OpenConnectionAsync();

                            foreach (var cotDien in dicPK.Values.Select(it => it.CotDien).ToList())
                            {
                                maCot.Value = cotDien.MA_COT;
                                long_.Value = cotDien.LONG_;
                                lat.Value = cotDien.LAT;
                                congDungCot.Value = cotDien.CONGDUNGCOT;
                                chieuCao.Value = cotDien.CHIEUCAO;
                                thuTuPha.Value = cotDien.THUTU_PHA;
                                soMachDD.Value = cotDien.SOMACH_DD;
                                trongLuong.Value = cotDien.TRONGLUONG;
                                soHuu.Value = cotDien.SOHUU;
                                soMachDCS.Value = cotDien.SOMACH_DCS;
                                loaiMC.Value = cotDien.LOAI_MC;
                                loaiBLNM.Value = cotDien.LOAI_BLNM;
                                keMong.Value = cotDien.KEMONG;
                                tinh.Value = cotDien.TINH;
                                huyen.Value = cotDien.HUYEN;
                                xa.Value = cotDien.XA;
                                loaiTD.Value = cotDien.LOAI_TD;
                                maDVQL.Value = cotDien.MADVQL;
                                maKVHC.Value = cotDien.MAKVHC;
                                capDA.Value = cotDien.CAPDA;
                                tenTTD.Value = cotDien.TEN_TTD;
                                maViTri.Value = cotDien.MAVITRI;
                                tenViTri.Value = cotDien.TENVITRI;
                                maTiepDia.Value = cotDien.MATIEPDIA;
                                tenCot.Value = cotDien.TEN_COT;
                                maDCS.Value = cotDien.MA_DCS;
                                maCT.Value = cotDien.MACT;
                                tenCT.Value = cotDien.TENCT;
                                maTTDKV.Value = cotDien.MATTDKV;
                                numRowAdd += await command.ExecuteNonQueryAsync();
                            }
                        }
                        break;
                }
                _logger.LogInformation($"Số bản ghi được thêm vào bảng {tableName}: {numRowAdd}");
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

        private string GenInsertCotDien(List<EN_COTDIEN> cotDiens)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("BEGIN");
            //var cotDien = cotDiens.FirstOrDefault();
            for(var i = 0; i < 2; i++)
            {
                var cotDien = cotDiens[i];
                sb.AppendLine(string.Format(_insertCotDien,
                    cotDien.MA_COT,
                    cotDien.LONG_ == null ? "NULL" : cotDien.LONG_.Value.ToString(),
                    cotDien.LAT == null ? "NULL" : cotDien.LAT.Value.ToString(),
                    cotDien.CONGDUNGCOT,
                    cotDien.CHIEUCAO == null ? "NULL" : cotDien.CHIEUCAO.Value.ToString(),
                    cotDien.THUTU_PHA,
                    cotDien.SOMACH_DD == null ? "NULL" : cotDien.SOMACH_DD.Value.ToString(),
                    cotDien.TRONGLUONG == null ? "NULL" : cotDien.TRONGLUONG.Value.ToString(),
                    cotDien.SOHUU,
                    cotDien.SOMACH_DCS == null ? "NULL" : cotDien.SOMACH_DCS.Value.ToString(),
                    cotDien.LOAI_MC,
                    cotDien.LOAI_BLNM,
                    cotDien.KEMONG,
                    cotDien.TINH,
                    cotDien.HUYEN,
                    cotDien.XA,
                    cotDien.LOAI_TD,
                    cotDien.MADVQL,
                    cotDien.MAKVHC,
                    cotDien.CAPDA,
                    cotDien.TEN_TTD,
                    cotDien.MAVITRI,
                    cotDien.TENVITRI,
                    cotDien.MATIEPDIA,
                    cotDien.TEN_COT,
                    cotDien.MA_DCS,
                    cotDien.MACT,
                    cotDien.TENCT,
                    cotDien.MATTDKV));
            }
            sb.AppendLine("END;");
            //foreach (var cotDien in cotDiens)
            //{

            //}
            return sb.ToString();
        }

    }
}

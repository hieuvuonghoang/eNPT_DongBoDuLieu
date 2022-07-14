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
        private readonly string _insertTramBienAp;
        private readonly string _insertDuongDay;
        private readonly string _insertFullTextSearch;
        private readonly string _updateMaCotDayDan;
        private readonly string _updateMaCotDuongDayCot;
        private readonly string _updateMaTramTramBienApDuongDay;
        private readonly string _updateMaDuongDayDayDan;
        private readonly string _updateMaDuongDayTramBienApDuongDay;
        private readonly string _updateMaDuongDayDuongDayCot;
        private readonly string _getCountByLoaiDTFullTextSearch;
        private readonly string _getObjectIDFullTextSearch;
        private readonly string _deleteFullTextSearch;
        private readonly string _deleteCotDien;
        private readonly string _deleteTramBienAp;
        private readonly string _deleteDuongDay;

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
                               TENCT, MATTDKV, OBJECTID) 
                               VALUES (:MA_COT, :LONG_, :LAT, 
                                       :CONGDUNGCOT, :CHIEUCAO, :THUTU_PHA, 
                                       :SOMACH_DD, :TRONGLUONG, :SOHUU, 
                                       :SOMACH_DCS, :LOAI_MC, :LOAI_BLNM, 
                                       :KEMONG, :TINH, :HUYEN, 
                                       :XA, :LOAI_TD, :MADVQL, 
                                       :MAKVHC, :CAPDA, :TEN_TTD, 
                                       :MAVITRI, :TENVITRI, :MATIEPDIA, 
                                       :TEN_COT, :MA_DCS, :MACT, 
                                       :TENCT, :MATTDKV, :OBJECTID)";
            _insertFullTextSearch = @"INSERT INTO EVNNPT.EN_FULLTEXTSEARCH ( 
                                      FTSID, LOAIDT, DATA, 
                                      OBJECTID, MADT, TENDT, 
                                      MADVQL) 
                                      VALUES (:FTSID, :LOAIDT, :DATA, 
                                              :OBJECTID, :MADT, :TENDT, 
                                              :MADVQL)";
            _insertTramBienAp = @"INSERT INTO EVNNPT.EN_TRAMBIENAP ( 
                                  TEN_CONGTY, LOAI_TRAM, TEN_TRAM, 
                                  LONG_, LAT, KIEU_TRAM, 
                                  TINH, HUYEN, XA, 
                                  NAM_VH, TEN_TTD, MADVQL, 
                                  MAKVHC, CAPDA, MATRAM, 
                                  SOHUU, MATTDKV, OBJECTID) 
                                  VALUES (:TEN_CONGTY, :LOAI_TRAM, :TEN_TRAM, 
                                          :LONG_, :LAT, :KIEU_TRAM, 
                                          :TINH, :HUYEN, :XA, 
                                          :NAM_VH, :TEN_TTD, :MADVQL, 
                                          :MAKVHC, :CAPDA, :MATRAM, 
                                          :SOHUU, :MATTDKV, :OBJECTID)";
            _insertDuongDay = @"INSERT INTO EVNNPT.EN_DUONGDAY (
                                MADUONGDAY, TENDUONGDAY, CHIEUDAI_DD, 
                                GHICHU, MADVQL, MADUONGDAYCHINH, 
                                TEN_TTD, TEN_CONGTY, HANG_SX, 
                                NUOC_SX, DONGDIEN_DM, LOAI_CD, 
                                MA_CD, HANG_SX_CD, NUOC_SX_CD, 
                                CHIEUDAI_DRCD, MA_DCS, KYHIEU, 
                                DK_DAY, DK_LOI, CAUTAO_CD, 
                                SOLUONG, SOLUONG_MN, SOLUONG_KDV, 
                                SOLUONG_TBCB, DUONGDAY, VITRIDAT, 
                                CAPDA, TUTRAM, DENTRAM, 
                                TENDUONGDAYCHINH, MATTDKV, NAM_VH, OBJECTID) 
                                VALUES (:MADUONGDAY, :TENDUONGDAY, :CHIEUDAI_DD,
                                        :GHICHU, :MADVQL, :MADUONGDAYCHINH,
                                        :TEN_TTD, :TEN_CONGTY, :HANG_SX,
                                        :NUOC_SX, :DONGDIEN_DM, :LOAI_CD,
                                        :MA_CD, :HANG_SX_CD, :NUOC_SX_CD,
                                        :CHIEUDAI_DRCD, :MA_DCS, :KYHIEU,
                                        :DK_DAY, :DK_LOI, :CAUTAO_CD,
                                        :SOLUONG, :SOLUONG_MN, :SOLUONG_KDV,
                                        :SOLUONG_TBCB, :DUONGDAY, :VITRIDAT,
                                        :CAPDA, :TUTRAM, :DENTRAM,
                                        :TENDUONGDAYCHINH, :MATTDKV, :NAM_VH, :OBJECTID)";
            _updateMaCotDayDan = @"UPDATE EVNNPT.EN_DAYDAN 
                              SET MA_COT = :MA_COT_VALUE 
                              WHERE MA_COT = :MA_COT_CONDITION";
            _updateMaCotDuongDayCot = @"UPDATE EVNNPT.EN_DUONGDAY_COT 
                                   SET MA_COT = :MA_COT_VALUE 
                                   WHERE MA_COT = :MA_COT_CONDITION";
            _updateMaTramTramBienApDuongDay = @"UPDATE EVNNPT.EN_TRAMBIENAP_DUONGDAY 
                                   SET MATRAM = :MATRAM_VALUE 
                                   WHERE MATRAM = :MATRAM_CONDITION";
            _updateMaDuongDayDayDan = @"UPDATE EVNNPT.EN_DAYDAN 
                              SET MADUONGDAY = :MADUONGDAY_VALUE 
                              WHERE MADUONGDAY = :MADUONGDAY_CONDITION";
            _updateMaDuongDayTramBienApDuongDay = @"UPDATE EVNNPT.EN_TRAMBIENAP_DUONGDAY 
                                   SET MADUONGDAY = :MADUONGDAY_VALUE 
                                   WHERE MADUONGDAY = :MADUONGDAY_CONDITION";
            _updateMaDuongDayDuongDayCot = @"UPDATE EVNNPT.EN_DUONGDAY_COT 
                                   SET MADUONGDAY = :MADUONGDAY_VALUE 
                                   WHERE MADUONGDAY = :MADUONGDAY_CONDITION";
            _getCountByLoaiDTFullTextSearch = @"SELECT COUNT(FTSID) 
                                                FROM EVNNPT.EN_FULLTEXTSEARCH WHERE LOAIDT = :LOAIDT";
            _getObjectIDFullTextSearch = @"SELECT OBJECTID FROM (SELECT OBJECTID, ROWNUM AS RN FROM EVNNPT.EN_FULLTEXTSEARCH 
                                           WHERE LOAIDT = :LOAIDT ORDER BY FTSID) WHERE RN BETWEEN :RNSTART AND :RNEND";
            _deleteFullTextSearch = @"DELETE FROM EVNNPT.EN_FULLTEXTSEARCH WHERE LOAIDT = :LOAIDT AND OBJECTID = :OBJECTID";
            _deleteCotDien = @"DELETE FROM EVNNPT.EN_COTDIEN WHERE OBJECTID = :OBJECTID";
            _deleteTramBienAp = @"DELETE FROM EVNNPT.EN_TRAMBIENAP WHERE OBJECTID = :OBJECTID";
            _deleteDuongDay = @"DELETE FROM EVNNPT.EN_DUONGDAY WHERE OBJECTID = :OBJECTID";
        }

        public async Task DeleteFullTextSearchCotTramDuongDayAsync(ELoaiDT loaiDT, List<string> oIds)
        {
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    var transaction = await command.Connection.BeginTransactionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = _deleteFullTextSearch;
                    command.Transaction = transaction;

                    DbParameter LOAIDT = command.CreateParameter();
                    LOAIDT.DbType = DbType.String;
                    LOAIDT.ParameterName = "LOAIDT";
                    command.Parameters.Add(LOAIDT);

                    DbParameter OBJECTID = command.CreateParameter();
                    OBJECTID.DbType = DbType.String;
                    OBJECTID.ParameterName = "OBJECTID";
                    command.Parameters.Add(OBJECTID);

                    var numRowDelete = 0;

                    foreach (var oId in oIds)
                    {
                        LOAIDT.Value = $"{loaiDT}";
                        OBJECTID.Value = oId;
                        try
                        {
                            numRowDelete += await command.ExecuteNonQueryAsync();
                        } catch (Exception ex)
                        {
                            _logger.LogWarning($"Lỗi khi xóa dữ liệu FullTextSearch LOAIDT = {loaiDT}, OBJECTID = {oId}.");
                            _logger.LogError(ex.Message, ex);
                        }
                    }

                    switch(loaiDT)
                    {
                        case ELoaiDT.COT:
                            command.CommandText = _deleteCotDien;
                            break;
                        case ELoaiDT.DDA:
                            command.CommandText = _deleteDuongDay;
                            break;
                        case ELoaiDT.TBA:
                            command.CommandText = _deleteTramBienAp;
                            break;
                    }

                    command.Parameters.Clear();

                    DbParameter dOBJECTID = command.CreateParameter();
                    dOBJECTID.DbType = DbType.Decimal;
                    dOBJECTID.ParameterName = "OBJECTID";
                    command.Parameters.Add(dOBJECTID);

                    var numRowDeleteCotTramDuongDay = 0;

                    foreach (var oId in oIds)
                    {
                        dOBJECTID.Value = oId;
                        try
                        {
                            numRowDeleteCotTramDuongDay += await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning($"Lỗi khi xóa cột, trạm, hoặc đường dây: LOAIDT = {loaiDT}, OBJECTID = {oId}.");
                            _logger.LogError(ex.Message, ex);
                        }
                    }

                    await transaction.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        public async Task<List<string>> GetObjectIDFullTextSearchAsyncs(ELoaiDT loaiDT, int rNStart, int rNEnd)
        {
            var rets = new List<string>();
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = _getObjectIDFullTextSearch;

                    DbParameter LOAIDT = command.CreateParameter();
                    LOAIDT.DbType = DbType.String;
                    LOAIDT.ParameterName = "LOAIDT";
                    command.Parameters.Add(LOAIDT);

                    DbParameter RNSTART = command.CreateParameter();
                    RNSTART.DbType = DbType.Int32;
                    RNSTART.ParameterName = "RNSTART";
                    command.Parameters.Add(RNSTART);

                    DbParameter RNEND = command.CreateParameter();
                    RNEND.DbType = DbType.Int32;
                    RNEND.ParameterName = "RNEND";
                    command.Parameters.Add(RNEND);

                    LOAIDT.Value = $"{loaiDT}";
                    RNSTART.Value = rNStart;
                    RNEND.Value = rNEnd;

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (result.Read())
                        {
                            var objectId = result.GetString(0);
                            rets.Add(objectId);
                        }
                    }
                }
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return rets;
        }

        public async Task<int> GetCountByLoaiDTFullTextSearchAsync(ELoaiDT loaiDT)
        {
            var ret = 0;
            try
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    command.CommandType = System.Data.CommandType.Text;

                    command.CommandText = _getCountByLoaiDTFullTextSearch;

                    DbParameter LOAIDT = command.CreateParameter();
                    LOAIDT.DbType = DbType.String;
                    LOAIDT.ParameterName = "LOAIDT";
                    command.Parameters.Add(LOAIDT);

                    LOAIDT.Value = $"{loaiDT}";

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (result.Read())
                        {
                            ret = result.GetInt32(0);
                        }
                    }
                }
            } catch(Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return ret;
        }

        public async Task DeleteAddInsertFeaturesAsync(ELoaiDT loaiDT, List<EN_FULLTEXTSEARCH> fullTextSearchs)
        {
            try
            {
                var pKTables = "";
                var sQLDelete = "";
                var tableName = "";
                var dicPK = new Dictionary<string, EN_FULLTEXTSEARCH>();
                var dicObjectId = new Dictionary<decimal, EN_FULLTEXTSEARCH>();
                switch (loaiDT)
                {
                    case ELoaiDT.COT:
                        tableName = "EN_COTDIEN";
                        foreach (var fullTextSearch in fullTextSearchs)
                        {
                            if (!string.IsNullOrEmpty(fullTextSearch.CotDien.MA_COT) && !dicPK.ContainsKey(fullTextSearch.CotDien.MA_COT))
                            {
                                dicPK.Add(fullTextSearch.CotDien.MA_COT, fullTextSearch);
                            }
                            var objectId = fullTextSearch.CotDien.OBJECTID;
                            if (objectId != null && !dicObjectId.ContainsKey(objectId.Value))
                            {
                                dicObjectId.Add(objectId.Value, fullTextSearch);
                            }
                        }
                        var cotDiens = dicPK.Values.Select(it => it.CotDien).ToList();
                        //Danh sách cột điện có mã cột bị chỉnh sửa
                        var cotDienMaCotUpdates = await GetMaCotByObjectId(cotDiens);
                        foreach (var cotDienMaCotUpdate in cotDienMaCotUpdates)
                        {
                            if (cotDienMaCotUpdate.OBJECTID == null)
                            {
                                continue;
                            }
                            var objectId = cotDienMaCotUpdate.OBJECTID.Value;
                            if (dicObjectId.ContainsKey(objectId))
                            {
                                var fullTextSearch = dicObjectId[objectId];
                                cotDienMaCotUpdate.MA_COT_UPDATE = fullTextSearch.CotDien.MA_COT;
                            }
                        }
                        pKTables = string.Join(",", dicPK.Values.Select(it => $"'{it.CotDien.OBJECTID}'").ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE OBJECTID IN ({pKTables})";
                        await DeleteAndInsertCotDien(cotDiens, sQLDelete, cotDienMaCotUpdates);
                        break;
                    case ELoaiDT.DDA:
                        tableName = "EN_DUONGDAY";
                        foreach (var fullTextSearch in fullTextSearchs)
                        {
                            if (!string.IsNullOrEmpty(fullTextSearch.DuongDay.MADUONGDAY) && !dicPK.ContainsKey(fullTextSearch.DuongDay.MADUONGDAY))
                            {
                                dicPK.Add(fullTextSearch.DuongDay.MADUONGDAY, fullTextSearch);
                            }
                        }
                        var duongDays = dicPK.Values.Select(it => it.DuongDay).ToList();
                        //Danh sách đường dây có mã đường dây bị chỉnh sửa
                        var duongDayMaDuongDayUpdates = await GetMaDuongDayByObjectId(duongDays);
                        foreach (var duongDayMaDuongDayUpdate in duongDayMaDuongDayUpdates)
                        {
                            if (duongDayMaDuongDayUpdate.OBJECTID == null)
                            {
                                continue;
                            }
                            var objectId = duongDayMaDuongDayUpdate.OBJECTID.Value;
                            if (dicObjectId.ContainsKey(objectId))
                            {
                                var fullTextSearch = dicObjectId[objectId];
                                duongDayMaDuongDayUpdate.MADUONGDAY_UPDATE = fullTextSearch.DuongDay.MADUONGDAY;
                            }
                        }
                        pKTables = string.Join(",", fullTextSearchs.Select(it => $"'{it.DuongDay.OBJECTID}'").ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE OBJECTID IN ({pKTables})";
                        await DeleteAndInsertDuongDay(duongDays, sQLDelete, duongDayMaDuongDayUpdates);
                        break;
                    case ELoaiDT.TBA:
                        tableName = "EN_TRAMBIENAP";
                        foreach (var fullTextSearch in fullTextSearchs)
                        {
                            if (!string.IsNullOrEmpty(fullTextSearch.TramBienAp.MATRAM) && !dicPK.ContainsKey(fullTextSearch.TramBienAp.MATRAM))
                            {
                                dicPK.Add(fullTextSearch.TramBienAp.MATRAM, fullTextSearch);
                            }
                            var objectId = fullTextSearch.TramBienAp.OBJECTID;
                            if (objectId != null && !dicObjectId.ContainsKey(objectId.Value))
                            {
                                dicObjectId.Add(objectId.Value, fullTextSearch);
                            }
                        }
                        var tramBienAps = dicPK.Values.Select(it => it.TramBienAp).ToList();
                        //Danh sách trạm biến áp có mã trạm bị chỉnh sửa
                        var tramBienApMaTramUpdates = await GetMaTramByObjectId(tramBienAps);
                        foreach (var tramBienApMaTramUpdate in tramBienApMaTramUpdates)
                        {
                            if (tramBienApMaTramUpdate.OBJECTID == null)
                            {
                                continue;
                            }
                            var objectId = tramBienApMaTramUpdate.OBJECTID.Value;
                            if (dicObjectId.ContainsKey(objectId))
                            {
                                var fullTextSearch = dicObjectId[objectId];
                                tramBienApMaTramUpdate.MATRAM_UPDATE = fullTextSearch.TramBienAp.MATRAM;
                            }
                        }
                        pKTables = string.Join(",", fullTextSearchs.Select(it => $"'{it.TramBienAp.OBJECTID}'").ToList());
                        sQLDelete = $"DELETE FROM {tableName} WHERE OBJECTID IN ({pKTables})";
                        await DeleteAndInsertTramBienAp(tramBienAps, sQLDelete, tramBienApMaTramUpdates);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private async Task DeleteAndInsertCotDien(List<EN_COTDIEN> cotDiens, string sQLDelete, List<EN_COTDIEN> cotDienMaCotUpdates)
        {
            try
            {
                //Delete
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    var transaction = await command.Connection.BeginTransactionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.Transaction = transaction;

                    #region "Delete"
                    command.CommandText = sQLDelete;

                    var numRowDelete = await command.ExecuteNonQueryAsync();
                    #endregion

                    #region "Insert"

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

                    DbParameter pOBJECTID = command.CreateParameter();
                    pOBJECTID.DbType = DbType.Decimal;
                    pOBJECTID.ParameterName = "OBJECTID";
                    command.Parameters.Add(pOBJECTID);

                    #endregion

                    var numRowAdd = 0;

                    foreach (var cotDien in cotDiens)
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
                        pOBJECTID.Value = cotDien.OBJECTID;
                        try
                        {
                            numRowAdd += await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(JsonConvert.SerializeObject(cotDien));
                            _logger.LogError(ex.Message, ex);
                        }
                    }

                    #endregion

                    #region "Update dữ liệu các bảng liên quan khi trường MA_COT bị chỉnh sửa"

                    DbParameter MA_COT_VALUE = command.CreateParameter();
                    MA_COT_VALUE.DbType = DbType.String;
                    MA_COT_VALUE.ParameterName = "MA_COT_VALUE";
                    command.Parameters.Add(MA_COT_VALUE);

                    DbParameter MA_COT_CONDITION = command.CreateParameter();
                    MA_COT_CONDITION.DbType = DbType.String;
                    MA_COT_CONDITION.ParameterName = "MA_COT_CONDITION";
                    command.Parameters.Add(MA_COT_CONDITION);

                    #region "EN_DAYDAN"

                    command.CommandText = _updateMaCotDayDan;

                    var numRowUpdateDayDan = 0;

                    foreach (var cotDienMaCotUpdate in cotDienMaCotUpdates)
                    {
                        MA_COT_VALUE.Value = cotDienMaCotUpdate.MA_COT_UPDATE;
                        MA_COT_CONDITION.Value = cotDienMaCotUpdate.MA_COT;
                        numRowUpdateDayDan += await command.ExecuteNonQueryAsync();
                    }

                    #endregion

                    #region "EN_DUONGDAY_COT"

                    command.CommandText = _updateMaCotDuongDayCot;

                    var numRowUpdateDuongDayCot = 0;

                    foreach (var cotDienMaCotUpdate in cotDienMaCotUpdates)
                    {
                        MA_COT_VALUE.Value = cotDienMaCotUpdate.MA_COT_UPDATE;
                        MA_COT_CONDITION.Value = cotDienMaCotUpdate.MA_COT;
                        numRowUpdateDuongDayCot += await command.ExecuteNonQueryAsync();
                    }

                    #endregion

                    #endregion

                    await transaction.CommitAsync();

                    //_logger.LogInformation($"Số bản ghi đã xóa trong bảng EN_COTDIEN: {numRowDelete}");
                    //_logger.LogInformation($"Số bản ghi đã thêm mới vào bảng EN_COTDIEN: {numRowAdd}");
                    //_logger.LogInformation($"Số bản ghi đã cập nhật vào bảng EN_DAYDAN: {numRowUpdateDayDan}");
                    //_logger.LogInformation($"Số bản ghi đã cập nhật vào bảng EN_DUONGDAY_COT: {numRowUpdateDuongDayCot}");

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private async Task DeleteAndInsertTramBienAp(List<EN_TRAMBIENAP> tramBienAps, string sQLDelete, List<EN_TRAMBIENAP> tramBienApUpdates)
        {
            try
            {
                //Delete
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    var transaction = await command.Connection.BeginTransactionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.Transaction = transaction;

                    #region "Delete"
                    command.CommandText = sQLDelete;

                    var numRowDelete = await command.ExecuteNonQueryAsync();
                    #endregion

                    #region "Insert"

                    command.CommandText = _insertTramBienAp;

                    #region "DbParameter"
                    DbParameter pTEN_CONGTY = command.CreateParameter();
                    pTEN_CONGTY.DbType = DbType.String;
                    pTEN_CONGTY.ParameterName = "TEN_CONGTY";
                    command.Parameters.Add(pTEN_CONGTY);

                    DbParameter pLOAI_TRAM = command.CreateParameter();
                    pLOAI_TRAM.DbType = DbType.String;
                    pLOAI_TRAM.ParameterName = "LOAI_TRAM";
                    command.Parameters.Add(pLOAI_TRAM);

                    DbParameter pTEN_TRAM = command.CreateParameter();
                    pTEN_TRAM.DbType = DbType.String;
                    pTEN_TRAM.ParameterName = "TEN_TRAM";
                    command.Parameters.Add(pTEN_TRAM);

                    DbParameter pLONG_ = command.CreateParameter();
                    pLONG_.DbType = DbType.Decimal;
                    pLONG_.ParameterName = "LONG_";
                    command.Parameters.Add(pLONG_);

                    DbParameter pLAT = command.CreateParameter();
                    pLAT.DbType = DbType.Decimal;
                    pLAT.ParameterName = "LAT";
                    command.Parameters.Add(pLAT);

                    DbParameter pKIEU_TRAM = command.CreateParameter();
                    pKIEU_TRAM.DbType = DbType.String;
                    pKIEU_TRAM.ParameterName = "KIEU_TRAM";
                    command.Parameters.Add(pKIEU_TRAM);

                    DbParameter pTINH = command.CreateParameter();
                    pTINH.DbType = DbType.String;
                    pTINH.ParameterName = "TINH";
                    command.Parameters.Add(pTINH);

                    DbParameter pHUYEN = command.CreateParameter();
                    pHUYEN.DbType = DbType.String;
                    pHUYEN.ParameterName = "HUYEN";
                    command.Parameters.Add(pHUYEN);

                    DbParameter pXA = command.CreateParameter();
                    pXA.DbType = DbType.String;
                    pXA.ParameterName = "XA";
                    command.Parameters.Add(pXA);

                    DbParameter pNAM_VH = command.CreateParameter();
                    pNAM_VH.DbType = DbType.Int32;
                    pNAM_VH.ParameterName = "NAM_VH";
                    command.Parameters.Add(pNAM_VH);

                    DbParameter pTEN_TTD = command.CreateParameter();
                    pTEN_TTD.DbType = DbType.String;
                    pTEN_TTD.ParameterName = "TEN_TTD";
                    command.Parameters.Add(pTEN_TTD);

                    DbParameter pMADVQL = command.CreateParameter();
                    pMADVQL.DbType = DbType.String;
                    pMADVQL.ParameterName = "MADVQL";
                    command.Parameters.Add(pMADVQL);

                    DbParameter pMAKVHC = command.CreateParameter();
                    pMAKVHC.DbType = DbType.String;
                    pMAKVHC.ParameterName = "MAKVHC";
                    command.Parameters.Add(pMAKVHC);

                    DbParameter pCAPDA = command.CreateParameter();
                    pCAPDA.DbType = DbType.String;
                    pCAPDA.ParameterName = "CAPDA";
                    command.Parameters.Add(pCAPDA);

                    DbParameter pMATRAM = command.CreateParameter();
                    pMATRAM.DbType = DbType.String;
                    pMATRAM.ParameterName = "MATRAM";
                    command.Parameters.Add(pMATRAM);

                    DbParameter pSOHUU = command.CreateParameter();
                    pSOHUU.DbType = DbType.String;
                    pSOHUU.ParameterName = "SOHUU";
                    command.Parameters.Add(pSOHUU);

                    DbParameter pMATTDKV = command.CreateParameter();
                    pMATTDKV.DbType = DbType.String;
                    pMATTDKV.ParameterName = "MATTDKV";
                    command.Parameters.Add(pMATTDKV);

                    DbParameter pOBJECTID = command.CreateParameter();
                    pOBJECTID.DbType = DbType.Decimal;
                    pOBJECTID.ParameterName = "OBJECTID";
                    command.Parameters.Add(pOBJECTID);

                    #endregion

                    var numRowAdd = 0;

                    foreach (var tramBienAp in tramBienAps)
                    {
                        pTEN_CONGTY.Value = tramBienAp.TEN_CONGTY;
                        pLOAI_TRAM.Value = tramBienAp.LOAI_TRAM;
                        pTEN_TRAM.Value = tramBienAp.TEN_TRAM;
                        pLONG_.Value = tramBienAp.LONG_;
                        pLAT.Value = tramBienAp.LAT;
                        pKIEU_TRAM.Value = tramBienAp.KIEU_TRAM;
                        pTINH.Value = tramBienAp.TINH;
                        pHUYEN.Value = tramBienAp.HUYEN;
                        pXA.Value = tramBienAp.XA;
                        pNAM_VH.Value = tramBienAp.NAM_VH;
                        pTEN_TTD.Value = tramBienAp.TEN_TTD;
                        pMADVQL.Value = tramBienAp.MADVQL;
                        pMAKVHC.Value = tramBienAp.MAKVHC;
                        pCAPDA.Value = tramBienAp.CAPDA;
                        pMATRAM.Value = tramBienAp.MATRAM;
                        pSOHUU.Value = tramBienAp.SOHUU;
                        pMATTDKV.Value = tramBienAp.MATTDKV;
                        pOBJECTID.Value = tramBienAp.OBJECTID;
                        try
                        {
                            numRowAdd += await command.ExecuteNonQueryAsync();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(JsonConvert.SerializeObject(tramBienAp));
                            _logger.LogError(ex.Message, ex);
                        }
                        
                    }

                    #endregion

                    #region "Update dữ liệu các bảng liên quan khi trường MATRAM bị chỉnh sửa"

                    DbParameter MATRAM_VALUE = command.CreateParameter();
                    MATRAM_VALUE.DbType = DbType.String;
                    MATRAM_VALUE.ParameterName = "MATRAM_VALUE";
                    command.Parameters.Add(MATRAM_VALUE);

                    DbParameter MATRAM_CONDITION = command.CreateParameter();
                    MATRAM_CONDITION.DbType = DbType.String;
                    MATRAM_CONDITION.ParameterName = "MATRAM_CONDITION";
                    command.Parameters.Add(MATRAM_CONDITION);

                    #region "EN_TRAMBIENAP_DUONGDAY"

                    command.CommandText = _updateMaTramTramBienApDuongDay;

                    var numRowUpdateTramBienApDuongDay = 0;

                    foreach (var tramBienApUpdate in tramBienApUpdates)
                    {
                        MATRAM_VALUE.Value = tramBienApUpdate.MATRAM_UPDATE;
                        MATRAM_CONDITION.Value = tramBienApUpdate.MATRAM;
                        numRowUpdateTramBienApDuongDay += await command.ExecuteNonQueryAsync();
                    }

                    #endregion

                    #endregion

                    await transaction.CommitAsync();

                    //_logger.LogInformation($"Số bản ghi đã xóa trong bảng EN_TRAMBIENAP: {numRowDelete}");
                    //_logger.LogInformation($"Số bản ghi đã thêm mới vào bảng EN_TRAMBIENAP: {numRowAdd}");
                    //_logger.LogInformation($"Số bản ghi đã cập nhật vào bảng EN_TRAMBIENAP_DUONGDAY: {numRowUpdateTramBienApDuongDay}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
        }

        private async Task DeleteAndInsertDuongDay(List<EN_DUONGDAY> duongDays, string sQLDelete, List<EN_DUONGDAY> duongDayUpdates)
        {
            try
            {
                //Delete
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    var transaction = await command.Connection.BeginTransactionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.Transaction = transaction;

                    #region "Delete"
                    command.CommandText = sQLDelete;

                    var numRowDelete = await command.ExecuteNonQueryAsync();
                    #endregion

                    #region "Insert"

                    command.CommandText = _insertDuongDay;

                    #region "DbParameter"
                    DbParameter MADUONGDAY = command.CreateParameter();
                    MADUONGDAY.DbType = DbType.String;
                    MADUONGDAY.ParameterName = "MADUONGDAY";
                    command.Parameters.Add(MADUONGDAY);

                    DbParameter TENDUONGDAY = command.CreateParameter();
                    TENDUONGDAY.DbType = DbType.String;
                    TENDUONGDAY.ParameterName = "TENDUONGDAY";
                    command.Parameters.Add(TENDUONGDAY);

                    DbParameter CHIEUDAI_DD = command.CreateParameter();
                    CHIEUDAI_DD.DbType = DbType.Decimal;
                    CHIEUDAI_DD.ParameterName = "CHIEUDAI_DD";
                    command.Parameters.Add(CHIEUDAI_DD);

                    DbParameter GHICHU = command.CreateParameter();
                    GHICHU.DbType = DbType.String;
                    GHICHU.ParameterName = "GHICHU";
                    command.Parameters.Add(GHICHU);

                    DbParameter MADVQL = command.CreateParameter();
                    MADVQL.DbType = DbType.String;
                    MADVQL.ParameterName = "MADVQL";
                    command.Parameters.Add(MADVQL);

                    DbParameter MADUONGDAYCHINH = command.CreateParameter();
                    MADUONGDAYCHINH.DbType = DbType.String;
                    MADUONGDAYCHINH.ParameterName = "MADUONGDAYCHINH";
                    command.Parameters.Add(MADUONGDAYCHINH);

                    DbParameter TEN_TTD = command.CreateParameter();
                    TEN_TTD.DbType = DbType.String;
                    TEN_TTD.ParameterName = "TEN_TTD";
                    command.Parameters.Add(TEN_TTD);

                    DbParameter TEN_CONGTY = command.CreateParameter();
                    TEN_CONGTY.DbType = DbType.String;
                    TEN_CONGTY.ParameterName = "TEN_CONGTY";
                    command.Parameters.Add(TEN_CONGTY);

                    DbParameter HANG_SX = command.CreateParameter();
                    HANG_SX.DbType = DbType.String;
                    HANG_SX.ParameterName = "HANG_SX";
                    command.Parameters.Add(HANG_SX);

                    DbParameter NUOC_SX = command.CreateParameter();
                    NUOC_SX.DbType = DbType.String;
                    NUOC_SX.ParameterName = "NUOC_SX";
                    command.Parameters.Add(NUOC_SX);

                    DbParameter DONGDIEN_DM = command.CreateParameter();
                    DONGDIEN_DM.DbType = DbType.Decimal;
                    DONGDIEN_DM.ParameterName = "DONGDIEN_DM";
                    command.Parameters.Add(DONGDIEN_DM);

                    DbParameter LOAI_CD = command.CreateParameter();
                    LOAI_CD.DbType = DbType.String;
                    LOAI_CD.ParameterName = "LOAI_CD";
                    command.Parameters.Add(LOAI_CD);

                    DbParameter MA_CD = command.CreateParameter();
                    MA_CD.DbType = DbType.String;
                    MA_CD.ParameterName = "MA_CD";
                    command.Parameters.Add(MA_CD);

                    DbParameter HANG_SX_CD = command.CreateParameter();
                    HANG_SX_CD.DbType = DbType.String;
                    HANG_SX_CD.ParameterName = "HANG_SX_CD";
                    command.Parameters.Add(HANG_SX_CD);

                    DbParameter NUOC_SX_CD = command.CreateParameter();
                    NUOC_SX_CD.DbType = DbType.String;
                    NUOC_SX_CD.ParameterName = "NUOC_SX_CD";
                    command.Parameters.Add(NUOC_SX_CD);

                    DbParameter CHIEUDAI_DRCD = command.CreateParameter();
                    CHIEUDAI_DRCD.DbType = DbType.Decimal;
                    CHIEUDAI_DRCD.ParameterName = "CHIEUDAI_DRCD";
                    command.Parameters.Add(CHIEUDAI_DRCD);

                    DbParameter MA_DCS = command.CreateParameter();
                    MA_DCS.DbType = DbType.String;
                    MA_DCS.ParameterName = "MA_DCS";
                    command.Parameters.Add(MA_DCS);

                    DbParameter KYHIEU = command.CreateParameter();
                    KYHIEU.DbType = DbType.String;
                    KYHIEU.ParameterName = "KYHIEU";
                    command.Parameters.Add(KYHIEU);

                    DbParameter DK_DAY = command.CreateParameter();
                    DK_DAY.DbType = DbType.Decimal;
                    DK_DAY.ParameterName = "DK_DAY";
                    command.Parameters.Add(DK_DAY);

                    DbParameter DK_LOI = command.CreateParameter();
                    DK_LOI.DbType = DbType.Decimal;
                    DK_LOI.ParameterName = "DK_LOI";
                    command.Parameters.Add(DK_LOI);

                    DbParameter CAUTAO_CD = command.CreateParameter();
                    CAUTAO_CD.DbType = DbType.String;
                    CAUTAO_CD.ParameterName = "CAUTAO_CD";
                    command.Parameters.Add(CAUTAO_CD);

                    DbParameter SOLUONG = command.CreateParameter();
                    SOLUONG.DbType = DbType.Decimal;
                    SOLUONG.ParameterName = "SOLUONG";
                    command.Parameters.Add(SOLUONG);

                    DbParameter SOLUONG_MN = command.CreateParameter();
                    SOLUONG_MN.DbType = DbType.Decimal;
                    SOLUONG_MN.ParameterName = "SOLUONG_MN";
                    command.Parameters.Add(SOLUONG_MN);

                    DbParameter SOLUONG_KDV = command.CreateParameter();
                    SOLUONG_KDV.DbType = DbType.Decimal;
                    SOLUONG_KDV.ParameterName = "SOLUONG_KDV";
                    command.Parameters.Add(SOLUONG_KDV);

                    DbParameter SOLUONG_TBCB = command.CreateParameter();
                    SOLUONG_TBCB.DbType = DbType.Decimal;
                    SOLUONG_TBCB.ParameterName = "SOLUONG_TBCB";
                    command.Parameters.Add(SOLUONG_TBCB);

                    DbParameter DUONGDAY = command.CreateParameter();
                    DUONGDAY.DbType = DbType.String;
                    DUONGDAY.ParameterName = "DUONGDAY";
                    command.Parameters.Add(DUONGDAY);

                    DbParameter VITRIDAT = command.CreateParameter();
                    VITRIDAT.DbType = DbType.String;
                    VITRIDAT.ParameterName = "VITRIDAT";
                    command.Parameters.Add(VITRIDAT);

                    DbParameter CAPDA = command.CreateParameter();
                    CAPDA.DbType = DbType.String;
                    CAPDA.ParameterName = "CAPDA";
                    command.Parameters.Add(CAPDA);

                    DbParameter TUTRAM = command.CreateParameter();
                    TUTRAM.DbType = DbType.String;
                    TUTRAM.ParameterName = "TUTRAM";
                    command.Parameters.Add(TUTRAM);

                    DbParameter DENTRAM = command.CreateParameter();
                    DENTRAM.DbType = DbType.String;
                    DENTRAM.ParameterName = "DENTRAM";
                    command.Parameters.Add(DENTRAM);

                    DbParameter TENDUONGDAYCHINH = command.CreateParameter();
                    TENDUONGDAYCHINH.DbType = DbType.String;
                    TENDUONGDAYCHINH.ParameterName = "TENDUONGDAYCHINH";
                    command.Parameters.Add(TENDUONGDAYCHINH);

                    DbParameter MATTDKV = command.CreateParameter();
                    MATTDKV.DbType = DbType.String;
                    MATTDKV.ParameterName = "MATTDKV";
                    command.Parameters.Add(MATTDKV);

                    DbParameter NAM_VH = command.CreateParameter();
                    NAM_VH.DbType = DbType.DateTime;
                    NAM_VH.ParameterName = "NAM_VH";
                    command.Parameters.Add(NAM_VH);

                    DbParameter OBJECTID = command.CreateParameter();
                    OBJECTID.DbType = DbType.Decimal;
                    OBJECTID.ParameterName = "OBJECTID";
                    command.Parameters.Add(OBJECTID);

                    #endregion

                    var numRowAdd = 0;

                    foreach (var duongDay in duongDays)
                    {
                        MADUONGDAY.Value = duongDay.MADUONGDAY;
                        TENDUONGDAY.Value = duongDay.TENDUONGDAY;
                        CHIEUDAI_DD.Value = duongDay.CHIEUDAI_DD;
                        GHICHU.Value = duongDay.GHICHU;
                        MADVQL.Value = duongDay.MADVQL;
                        MADUONGDAYCHINH.Value = duongDay.MADUONGDAYCHINH;
                        TEN_TTD.Value = duongDay.TEN_TTD;
                        TEN_CONGTY.Value = duongDay.TEN_CONGTY;
                        HANG_SX.Value = duongDay.HANG_SX;
                        NUOC_SX.Value = duongDay.NUOC_SX;
                        DONGDIEN_DM.Value = duongDay.DONGDIEN_DM;
                        LOAI_CD.Value = duongDay.LOAI_CD;
                        MA_CD.Value = duongDay.MA_CD;
                        HANG_SX_CD.Value = duongDay.HANG_SX_CD;
                        NUOC_SX_CD.Value = duongDay.NUOC_SX_CD;
                        CHIEUDAI_DRCD.Value = duongDay.CHIEUDAI_DRCD;
                        MA_DCS.Value = duongDay.MA_DCS;
                        KYHIEU.Value = duongDay.KYHIEU;
                        DK_DAY.Value = duongDay.DK_DAY;
                        DK_LOI.Value = duongDay.DK_LOI;
                        CAUTAO_CD.Value = duongDay.CAUTAO_CD;
                        SOLUONG.Value = duongDay.SOLUONG;
                        SOLUONG_MN.Value = duongDay.SOLUONG_MN;
                        SOLUONG_KDV.Value = duongDay.SOLUONG_KDV;
                        SOLUONG_TBCB.Value = duongDay.SOLUONG_TBCB;
                        DUONGDAY.Value = duongDay.DUONGDAY;
                        VITRIDAT.Value = duongDay.VITRIDAT;
                        CAPDA.Value = duongDay.CAPDA;
                        TUTRAM.Value = duongDay.TUTRAM;
                        DENTRAM.Value = duongDay.DENTRAM;
                        TENDUONGDAYCHINH.Value = duongDay.TENDUONGDAYCHINH;
                        MATTDKV.Value = duongDay.MATTDKV;
                        NAM_VH.Value = duongDay.NAM_VH;
                        OBJECTID.Value = duongDay.OBJECTID;
                        try
                        {
                            numRowAdd += await command.ExecuteNonQueryAsync();
                        } catch (Exception ex)
                        {
                            _logger.LogWarning(JsonConvert.SerializeObject(duongDay));
                            _logger.LogError(ex.Message, ex);
                        }
                        
                    }

                    #endregion

                    #region "Update dữ liệu các bảng liên quan khi trường MADUONGDAY bị chỉnh sửa"

                    DbParameter MADUONGDAY_VALUE = command.CreateParameter();
                    MADUONGDAY_VALUE.DbType = DbType.String;
                    MADUONGDAY_VALUE.ParameterName = "MADUONGDAY_VALUE";
                    command.Parameters.Add(MADUONGDAY_VALUE);

                    DbParameter MADUONGDAY_CONDITION = command.CreateParameter();
                    MADUONGDAY_CONDITION.DbType = DbType.String;
                    MADUONGDAY_CONDITION.ParameterName = "MADUONGDAY_CONDITION";
                    command.Parameters.Add(MADUONGDAY_CONDITION);

                    #region "EN_DAYDAN"

                    command.CommandText = _updateMaDuongDayDayDan;

                    var numRowUpdateDayDan = 0;

                    foreach (var duongDayUpdate in duongDayUpdates)
                    {
                        MADUONGDAY_VALUE.Value = duongDayUpdate.MADUONGDAY_UPDATE;
                        MADUONGDAY_CONDITION.Value = duongDayUpdate.MADUONGDAY;
                        numRowUpdateDayDan += await command.ExecuteNonQueryAsync();
                    }

                    #endregion

                    #region "EN_DUONGDAY_COT"

                    command.CommandText = _updateMaDuongDayDuongDayCot;

                    var numRowUpdateDuongDayCot = 0;

                    foreach (var duongDayUpdate in duongDayUpdates)
                    {
                        MADUONGDAY_VALUE.Value = duongDayUpdate.MADUONGDAY_UPDATE;
                        MADUONGDAY_CONDITION.Value = duongDayUpdate.MADUONGDAY;
                        numRowUpdateDuongDayCot += await command.ExecuteNonQueryAsync();
                    }

                    #endregion

                    #region "EN_TRAMBIENAP_DUONGDAY"
                    command.CommandText = _updateMaDuongDayTramBienApDuongDay;

                    var numRowUpdateTramBienApDuongDay = 0;

                    foreach (var duongDayUpdate in duongDayUpdates)
                    {
                        MADUONGDAY_VALUE.Value = duongDayUpdate.MADUONGDAY_UPDATE;
                        MADUONGDAY_CONDITION.Value = duongDayUpdate.MADUONGDAY;
                        numRowUpdateTramBienApDuongDay += await command.ExecuteNonQueryAsync();
                    }
                    #endregion

                    #endregion

                    await transaction.CommitAsync();

                    //_logger.LogInformation($"Số bản ghi đã xóa trong bảng EN_DUONGDAY: {numRowDelete}");
                    //_logger.LogInformation($"Số bản ghi đã thêm mới vào bảng EN_DUONGDAY: {numRowAdd}");
                    //_logger.LogInformation($"Số bản ghi đã cập nhật vào bảng EN_DAYDAN: {numRowUpdateDayDan}");
                    //_logger.LogInformation($"Số bản ghi đã cập nhật vào bảng EN_DUONGDAY_COT: {numRowUpdateDuongDayCot}");
                    //_logger.LogInformation($"Số bản ghi đã cập nhật vào bảng EN_TRAMBIENAP_DUONGDAY: {numRowUpdateTramBienApDuongDay}");
                }
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
                //Xử lý dữ liệu
                var features = JArray.Parse(strFeatures);
                var objIds = new List<int>();
                var fullTextSearchs = new List<EN_FULLTEXTSEARCH>();
                foreach (var feature in features)
                {
                    objIds.Add((int)feature["attributes"]["OBJECTID"]);
                    fullTextSearchs.Add(MapStrFeatureToFullTextSearch(loaiDT, feature["attributes"].ToString(), feature.ToString()));
                }
                var strObjIds = string.Join(",", objIds.Select(it => it).ToList());
                //Thực hiện xóa và thêm mới
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    var transaction = await command.Connection.BeginTransactionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.Transaction = transaction;

                    #region "Delete"
                    var sQLDelete = $"DELETE FROM EN_FULLTEXTSEARCH WHERE LOAIDT = '{loaiDT}' AND OBJECTID IN ({strObjIds})";
                    command.CommandText = sQLDelete;

                    var numRowDelete = await command.ExecuteNonQueryAsync();
                    #endregion

                    #region "Insert"

                    command.CommandText = _insertFullTextSearch;

                    #region "Parameter"

                    DbParameter pFTSID = command.CreateParameter();
                    pFTSID.DbType = DbType.String;
                    pFTSID.ParameterName = "FTSID";
                    command.Parameters.Add(pFTSID);

                    DbParameter pLOAIDT = command.CreateParameter();
                    pLOAIDT.DbType = DbType.String;
                    pLOAIDT.ParameterName = "LOAIDT";
                    command.Parameters.Add(pLOAIDT);

                    DbParameter pDATA = command.CreateParameter();
                    pDATA.DbType = DbType.String;
                    pDATA.ParameterName = "DATA";
                    command.Parameters.Add(pDATA);

                    DbParameter pOBJECTID = command.CreateParameter();
                    pOBJECTID.DbType = DbType.String;
                    pOBJECTID.ParameterName = "OBJECTID";
                    command.Parameters.Add(pOBJECTID);

                    DbParameter pMADT = command.CreateParameter();
                    pMADT.DbType = DbType.String;
                    pMADT.ParameterName = "MADT";
                    command.Parameters.Add(pMADT);

                    DbParameter pTENDT = command.CreateParameter();
                    pTENDT.DbType = DbType.String;
                    pTENDT.ParameterName = "TENDT";
                    command.Parameters.Add(pTENDT);

                    DbParameter pMADVQL = command.CreateParameter();
                    pMADVQL.DbType = DbType.String;
                    pMADVQL.ParameterName = "MADVQL";
                    command.Parameters.Add(pMADVQL);

                    #endregion

                    var numRowAdd = 0;

                    foreach (var fullTextSearch in fullTextSearchs)
                    {
                        pFTSID.Value = Guid.NewGuid().ToString();
                        pLOAIDT.Value = fullTextSearch.LOAIDT;
                        pDATA.Value = fullTextSearch.DATA;
                        pOBJECTID.Value = fullTextSearch.OBJECTID;
                        pMADT.Value = fullTextSearch.MADT;
                        pTENDT.Value = fullTextSearch.TENDT;
                        pMADVQL.Value = fullTextSearch.MADVQL;

                        numRowAdd += await command.ExecuteNonQueryAsync();
                    }

                    #endregion

                    await transaction.CommitAsync();

                    //_logger.LogInformation($"Số bản ghi đã xóa trong bảng EN_FULLTEXTSEARCH: {numRowDelete}");
                    //_logger.LogInformation($"Số bản ghi đã thêm vào trong bảng EN_FULLTEXTSEARCH: {numRowAdd}");
                }
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

        /// <summary>
        /// Truy vấn lấy thông tin trường MA_COT, OBJECTID
        /// </summary>
        /// <param name="cotDiens"></param>
        /// <returns></returns>
        private async Task<List<EN_COTDIEN>> GetMaCotByObjectId(List<EN_COTDIEN> cotDiens)
        {
            var rets = new List<EN_COTDIEN>();
            try
            {
                var strBuild = new StringBuilder("SELECT OBJECTID, MA_COT FROM EN_COTDIEN WHERE ");
                var strFormat = "(OBJECTID = '{0}' AND MA_COT <> '{1}')";
                var first = true;
                foreach (var cotDien in cotDiens)
                {
                    if (first)
                    {
                        strBuild.AppendLine(string.Format(strFormat, cotDien.OBJECTID, cotDien.MA_COT));
                        first = false;
                    }
                    else
                    {
                        strBuild.AppendLine(string.Format($"OR {strFormat}", cotDien.OBJECTID, cotDien.MA_COT));
                    }
                }
                var sQLQuery = strBuild.ToString();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = sQLQuery;

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (result.Read())
                        {
                            var objectId = result.GetDecimal(0);
                            var maCot = result.GetString(1);
                            var cotDien = new EN_COTDIEN();
                            cotDien.OBJECTID = objectId;
                            cotDien.MA_COT = maCot;
                            rets.Add(cotDien);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return rets;
        }

        /// <summary>
        /// Truy vấn lấy thông tin trường MATRAM, OBJECTID
        /// </summary>
        /// <param name="tramBienAps"></param>
        /// <returns></returns>
        private async Task<List<EN_TRAMBIENAP>> GetMaTramByObjectId(List<EN_TRAMBIENAP> tramBienAps)
        {
            var rets = new List<EN_TRAMBIENAP>();
            try
            {
                var strBuild = new StringBuilder("SELECT OBJECTID, MATRAM FROM EN_TRAMBIENAP WHERE ");
                var strFormat = "(OBJECTID = '{0}' AND MATRAM <> '{1}')";
                var first = true;
                foreach (var tramBienAp in tramBienAps)
                {
                    if (first)
                    {
                        strBuild.AppendLine(string.Format(strFormat, tramBienAp.OBJECTID, tramBienAp.MATRAM));
                        first = false;
                    }
                    else
                    {
                        strBuild.AppendLine(string.Format($"OR {strFormat}", tramBienAp.OBJECTID, tramBienAp.MATRAM));
                    }
                }
                var sQLQuery = strBuild.ToString();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = sQLQuery;

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (result.Read())
                        {
                            var objectId = result.GetDecimal(0);
                            var maTram = result.GetString(1);
                            var tramBienAp = new EN_TRAMBIENAP();
                            tramBienAp.OBJECTID = objectId;
                            tramBienAp.MATRAM = maTram;
                            rets.Add(tramBienAp);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return rets;
        }

        /// <summary>
        /// Truy vấn lấy thông tin trường MADUONGDAY, OBJECTID
        /// </summary>
        /// <param name="duongDays"></param>
        /// <returns></returns>
        private async Task<List<EN_DUONGDAY>> GetMaDuongDayByObjectId(List<EN_DUONGDAY> duongDays)
        {
            var rets = new List<EN_DUONGDAY>();
            try
            {
                var strBuild = new StringBuilder("SELECT OBJECTID, MADUONGDAY FROM EN_DUONGDAY WHERE ");
                var strFormat = "(OBJECTID = '{0}' AND MADUONGDAY <> '{1}')";
                var first = true;
                foreach (var duongDay in duongDays)
                {
                    if (first)
                    {
                        strBuild.AppendLine(string.Format(strFormat, duongDay.OBJECTID, duongDay.MADUONGDAY));
                        first = false;
                    }
                    else
                    {
                        strBuild.AppendLine(string.Format($"OR {strFormat}", duongDay.OBJECTID, duongDay.MADUONGDAY));
                    }
                }
                var sQLQuery = strBuild.ToString();
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    await _context.Database.OpenConnectionAsync();

                    command.CommandType = System.Data.CommandType.Text;
                    command.CommandText = sQLQuery;

                    using (var result = await command.ExecuteReaderAsync())
                    {
                        while (result.Read())
                        {
                            var objectId = result.GetDecimal(0);
                            var maDuongDay = result.GetString(1);
                            var duongDay = new EN_DUONGDAY();
                            duongDay.OBJECTID = objectId;
                            duongDay.MADUONGDAY = maDuongDay;
                            rets.Add(duongDay);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return rets;
        }

    }
}

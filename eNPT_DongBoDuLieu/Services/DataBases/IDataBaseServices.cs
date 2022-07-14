using eNPT_DongBoDuLieu.Models.DataBases.EVNNPT;
using eNPT_DongBoDuLieu.Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.DataBases
{
    public interface IDataBaseServices
    {
        /// <summary>
        /// - Thực hiện xóa và thêm mới bản ghi trong bảng EN_FULLTEXTSEARCH:
        /// <para>+ Xóa bản ghi theo trường LOAIDT và OBJECTID.</para>
        /// <para>+ Thêm mới bản ghi.</para>
        /// </summary>
        /// <param name="loaiDT">Loại đối tượng.</param>
        /// <param name="strFeatures">Dữ liệu truy vấn từ Feature Service trả về.</param>
        /// <returns></returns>
        Task DeleteAndInsertFullTextSearchAsync(ELoaiDT loaiDT, string strFeatures);
        /// <summary>
        /// - Thực hiện xóa và thêm mới bản ghi trong bảng EN_COTDIEN, EN_DUONGDAY, EN_TRAMBIENAP:
        /// <para>+ Xóa bản ghi theo trường MA_COT (EN_COTDIEN), MADUONGDAY (EN_DUONGDAY), MATRAM (EN_TRAMBIENAP).</para>
        /// <para>+ Thêm mới bản ghi.</para>     
        /// </summary>
        /// <param name="loaiDT">Loại đối tượng.</param>
        /// <param name="fullTextSearchs">Đối tượng chứa dữ liệu CotDien, DuongDay, TramBienAp.</param>
        /// <returns></returns>
        Task DeleteAddInsertFeaturesAsync(ELoaiDT loaiDT, List<EN_FULLTEXTSEARCH> fullTextSearchs);

        /// <summary>
        /// Thực hiện đếm số bản ghi theo loại đối tượng trong bảng EN_FULLTEXTSEARCH
        /// </summary>
        /// <param name="loaiDT">Loại đối tượng</param>
        /// <returns></returns>
        Task<int> GetCountByLoaiDTFullTextSearchAsync(ELoaiDT loaiDT);

        /// <summary>
        /// Thực hiện truy vấn lấy ObjectID theo loại đối tượng trong bảng EN_FULLTEXTSEARCH
        /// </summary>
        /// <param name="loaiDT">Loại đối tượng</param>
        /// <param name="rNStart">Phân trang: Row Start</param>
        /// <param name="rNEnd">Phân trang: Row End</param>
        /// <returns></returns>
        Task<List<string>> GetObjectIDFullTextSearchAsyncs(ELoaiDT loaiDT, int rNStart, int rNEnd);

        /// <summary>
        /// Thực hiện xóa theo loại đối tượng và objectid trong bảng EN_FULLTEXTSEARCH, EN_COTDIEN, EN_TRAMBIENAP, EN_DUONGDAY
        /// </summary>
        /// <param name="loaiDT">Loại đối tượng</param>
        /// <param name="oIds">Danh sách OBJECTID theo loại đối tượng</param>
        /// <returns></returns>
        Task DeleteFullTextSearchCotTramDuongDayAsync(ELoaiDT loaiDT, List<string> oIds);
    }
}

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
    }
}

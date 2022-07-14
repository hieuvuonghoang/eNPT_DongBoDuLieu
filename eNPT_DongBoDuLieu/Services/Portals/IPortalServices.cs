using eNPT_DongBoDuLieu.Models.Portals;
using eNPT_DongBoDuLieu.Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.Portals
{
    public interface IPortalServices
    {
        /// <summary>
        /// Generate Token từ UserName và Password.
        /// </summary>
        /// <returns>Trả về đối tượng ResultGenerateToken.</returns>
        /// <exception cref="GenerateToken thất bại."></exception>
        /// <exception cref="..."></exception>
        Task<ResultGenerateToken> GenerateTokenAsync();

        /// <summary>
        /// Get số bản ghi của feature theo trường LAST_EDITED_DATE.
        /// </summary>
        /// <param name="token">Token để xác thực với FeatureServices.</param>
        /// <param name="loaiDT">Loại đối tượng: Đường dây, trạm biến áp hoặc cột.</param>
        /// <param name="lastEditDate">Thông tin ngày giờ lần cuối service thực hiện truy vấn tới feature.</param>
        /// <returns>Trả về số bản ghi.</returns>
        /// <exception cref="Thực hiện GetCountByLastEditTime thất bại."></exception>
        /// <exception cref="..."></exception>
        Task<int> GetCountByLastEditTimeAsync(string token, ELoaiDT loaiDT, LastEditDate lastEditDate);

        /// <summary>
        /// Get feature theo trường LAST_EDITED_DATE.
        /// </summary>
        /// <param name="token">Token để xác thực với FeatureServices.</param>
        /// <param name="loaiDT">Loại đối tượng: Đường dây, trạm biến áp hoặc cột.</param>
        /// <param name="lastEditDate">Thông tin ngày giờ lần cuối service thực hiện truy vấn tới feature.</param>
        /// <param name="resultOffset">Bỏ qua n bản ghi.</param>
        /// <param name="resultRecordCount">Lấy n bản ghi.</param>
        /// <returns>Features dưới dạng string.</returns>
        /// <exception cref="Thực hiện GetFeatures thất bại."></exception>
        /// <exception cref="..."></exception>
        Task<string> GetFeatureAsyncs(string token, ELoaiDT loaiDT, LastEditDate lastEditDate, int resultOffset, int resultRecordCount);

        /// <summary>
        /// Get ObjectID
        /// </summary>
        /// <param name="token"></param>
        /// <param name="loaiDT"></param>
        /// <returns></returns>
        Task<List<int>> GetObjectIDByOIDAsyncs(string token, ELoaiDT loaiDT, List<string> oIds);
    }
}

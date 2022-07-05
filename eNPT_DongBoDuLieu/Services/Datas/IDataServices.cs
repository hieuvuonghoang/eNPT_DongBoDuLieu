using eNPT_DongBoDuLieu.Models.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eNPT_DongBoDuLieu.Services.Datas
{
    /// <summary>
    /// Sử dụng để đọc ghi dữ liệu ra tệp dưới dạng text với cấu trúc json.
    /// SemaphoreSlim được sử dụng để kiểm soát quyền truy cập vào tệp.
    /// </summary>
    public interface IDataServices
    {
        /// <summary>
        /// Gán dữ liệu trường 'NgayGio' = 'NgayGioHienTai' và ghi dữ liệu ra tệp.
        /// </summary>
        /// <param name="lastEditDate"></param>
        /// <returns></returns>
        Task WriteDataAsync(LastEditDate lastEditDates);
        /// <summary>
        /// Đọc dữ liệu từ tệp và khởi tạo trường 'NgayGioHienTai'.
        /// </summary>
        /// <returns></returns>
        Task<LastEditDate> ReadDataAsync();
    }
}

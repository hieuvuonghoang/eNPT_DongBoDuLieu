using eNPT_DongBoDuLieu.Models.JsonConverts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.Services
{
    public class LastEditDate
    {
        /// <summary>
        /// Ngày giờ lần cuối service thực hiện truy vấn tới feature.
        /// Định dạng đọc ghi từ tệp: 'yyyy-MM-dd HH:mm:ss'
        /// </summary>
        [JsonConverter(typeof(CustomDateTimeConverter))]
        public DateTime NgayGio { get; set; }
        /// <summary>
        /// Ngày giờ hiện tại.
        /// Dữ liệu này không được ghi ra tệp.
        /// Chỉ được khởi tạo và gán lại giá trị khi đọc ghi tệp.
        /// </summary>
        [JsonIgnore]
        public DateTime NgayGioHienTai { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace eNPT_DongBoDuLieu.Models
{
    public class AppSettings
    {
        /// <summary>
        /// Thời gian gọi lại services, đơn vị giây.
        /// </summary>
        public int RefreshTime { get; set; }
        /// <summary>
        /// Thời gian gọi lại services, đơn vị mili giây
        /// </summary>
        public int RefreshMilliSeconds => this.RefreshTime * 1000;
        /// <summary>
        /// Đường dẫn lưu thời gian lần cuối cùng services thực hiện truy vấn tời FeatureServices.
        /// </summary>
        public string PathFileLastEditDates { get; set; }
        /// <summary>
        /// Tên trường viết điều kiện truy vấn.
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// Số bản ghi tối đa trên một trang.
        /// </summary>
        public int MaxRecordPerPage { get; set; }
        /// <summary>
        /// Thời gian Token Portal hết hạn, đơn vị phút.
        /// Giá trị tối đa 15 ngày.
        /// </summary>
        public int ExpirationToken { get; set; }
    }
}

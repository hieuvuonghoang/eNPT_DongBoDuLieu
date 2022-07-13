using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class EN_COTDIEN
    {
        /// <summary>
        /// Trường dữ liệu được thêm vào phục vụ xử lý dữ liệu, không tồn tại trong CSDL.
        /// </summary>
        [NotMapped]
        public int OBJECTID { get; set; }
    }
}

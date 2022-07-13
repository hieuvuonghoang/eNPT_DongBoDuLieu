using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class EN_FULLTEXTSEARCH
    {
        /// <summary>
        /// Đối tượng được thêm vào phục vụ xử lý dữ liệu, không tồn tại trong CSDL.
        /// </summary>
        [NotMapped]
        public EN_COTDIEN CotDien { get; set; }
        /// <summary>
        /// Đối tượng được thêm vào phục vụ xử lý dữ liệu, không tồn tại trong CSDL.
        /// </summary>
        [NotMapped]
        public EN_DUONGDAY DuongDay { get; set; }
        /// <summary>
        /// Đối tượng được thêm vào phục vụ xử lý dữ liệu, không tồn tại trong CSDL.
        /// </summary>
        [NotMapped]
        public EN_TRAMBIENAP TramBienAp { get; set; }
    }
}

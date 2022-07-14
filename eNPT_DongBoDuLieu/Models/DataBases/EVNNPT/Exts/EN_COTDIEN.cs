using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class EN_COTDIEN
    {
        /// <summary>
        /// Mã cột điện update từ gdb
        /// </summary>
        [NotMapped]
        public string MA_COT_UPDATE { get; set; }
    }
}

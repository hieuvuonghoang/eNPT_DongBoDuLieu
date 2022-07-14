using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class EN_DUONGDAY
    {
        /// <summary>
        /// Mã đường dây update từ gdb
        /// </summary>
        [NotMapped]
        public string MADUONGDAY_UPDATE { get; set; }
    }
}

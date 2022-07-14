using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class EN_TRAMBIENAP
    {
        /// <summary>
        /// Mã trạm biến áp update từ gdb
        /// </summary>
        [NotMapped]
        public string MATRAM_UPDATE { get; set; }
    }
}

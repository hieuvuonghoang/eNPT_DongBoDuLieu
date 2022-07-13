using System;
using System.Collections.Generic;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.Portals
{
    public class ResultGenerateToken
    {
        public string token { get; set; }
        public long expires { get; set; }
        public bool ssl { get; set; }
    }
}

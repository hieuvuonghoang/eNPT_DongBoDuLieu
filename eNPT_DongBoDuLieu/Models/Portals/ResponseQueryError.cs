using System;
using System.Collections.Generic;
using System.Text;

namespace eNPT_DongBoDuLieu.Models.Portals
{
    public class ResponseQueryError
    {
        public Error error { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
        public List<string> details { get; set; }
    }

}

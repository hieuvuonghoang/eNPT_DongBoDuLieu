using eNPT_DongBoDuLieu.Models.JsonConverts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class EN_TRAMBIENAP
    {
        public string TEN_CONGTY { get; set; }
        public string LOAI_TRAM { get; set; }
        public string TEN_TRAM { get; set; }
        public decimal? LONG_ { get; set; }
        public decimal? LAT { get; set; }
        public string KIEU_TRAM { get; set; }
        public string TINH { get; set; }
        public string HUYEN { get; set; }
        public string XA { get; set; }
        [JsonConverter(typeof(CustomDateTimeUtcConverterGetYear))]
        public decimal? NAM_VH { get; set; }
        public string TEN_TTD { get; set; }
        public string MADVQL { get; set; }
        public string MAKVHC { get; set; }
        public string CAPDA { get; set; }
        public string MATRAM { get; set; }
        public string SOHUU { get; set; }
        public string MATTDKV { get; set; }
        public decimal? OBJECTID { get; set; }
    }
}

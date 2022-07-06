using eNPT_DongBoDuLieu.Models.JsonConverts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#nullable disable

namespace eNPT_DongBoDuLieu.Models.DataBases.EVNNPT
{
    public partial class EN_DUONGDAY
    {
        public string MADUONGDAY { get; set; }
        public string TENDUONGDAY { get; set; }
        public decimal? CHIEUDAI_DD { get; set; }
        public string GHICHU { get; set; }
        public string MADVQL { get; set; }
        public string MADUONGDAYCHINH { get; set; }
        public string TEN_TTD { get; set; }
        public string TEN_CONGTY { get; set; }
        public string HANG_SX { get; set; }
        public string NUOC_SX { get; set; }
        public decimal? DONGDIEN_DM { get; set; }
        public string LOAI_CD { get; set; }
        public string MA_CD { get; set; }
        public string HANG_SX_CD { get; set; }
        public string NUOC_SX_CD { get; set; }
        public decimal? CHIEUDAI_DRCD { get; set; }
        public string MA_DCS { get; set; }
        public string KYHIEU { get; set; }
        public decimal? DK_DAY { get; set; }
        public decimal? DK_LOI { get; set; }
        public string CAUTAO_CD { get; set; }
        public decimal? SOLUONG { get; set; }
        public decimal? SOLUONG_MN { get; set; }
        public decimal? SOLUONG_KDV { get; set; }
        public decimal? SOLUONG_TBCB { get; set; }
        public string DUONGDAY { get; set; }
        public string VITRIDAT { get; set; }
        public string CAPDA { get; set; }
        public string TUTRAM { get; set; }
        public string DENTRAM { get; set; }
        public string TENDUONGDAYCHINH { get; set; }
        public string MATTDKV { get; set; }
        [JsonConverter(typeof(CustomDateTimeUtcConverter))]
        public DateTime? NAM_VH { get; set; }
    }
}

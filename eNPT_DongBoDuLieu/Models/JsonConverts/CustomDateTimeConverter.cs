using Newtonsoft.Json.Converters;

namespace eNPT_DongBoDuLieu.Models.JsonConverts
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}

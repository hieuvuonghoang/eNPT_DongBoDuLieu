using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace eNPT_DongBoDuLieu.Models.JsonConverts
{
    public class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }

    public class CustomDateTimeUtcConverter : IsoDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(reader.Value is long)
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return start.AddMilliseconds((long)reader.Value).ToLocalTime();
            } else
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
        }
    }

    public class CustomDateTimeUtcConverterGetYear : IsoDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value is long)
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var dateTime = start.AddMilliseconds((long)reader.Value).ToLocalTime();
                return (decimal)dateTime.Year;
            }
            else
            {
                return base.ReadJson(reader, objectType, existingValue, serializer);
            }
        }
    }
}

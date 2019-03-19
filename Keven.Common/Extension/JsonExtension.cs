using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keven.Common.Extension
{
    public static class JsonExtension
    {
        public static string ToJson<T>(this T obj, Formatting formatting = Formatting.None)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, formatting);
            }
            catch
            {
                return "";
            }
        }

        public static string ToJsonWithOutNull<T>(this T entity, Formatting formatting = Formatting.None)
        {
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            return Newtonsoft.Json.JsonConvert.SerializeObject(entity, formatting, jsonSerializerSettings);
        }
        public static T FromJson<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }
    }
}

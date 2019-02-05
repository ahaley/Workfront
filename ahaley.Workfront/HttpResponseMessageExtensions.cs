using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ahaley.Workfront
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> ParseScalar<T>(this HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            JObject jObject = JObject.Parse(responseString);
            JToken data = jObject["data"];
            T obj = data.ToObject<T>();
            return obj;
        }

        public static async Task<T[]> ParseArray<T>(this HttpResponseMessage response)
        {
            string responseString = await response.Content.ReadAsStringAsync();
            JToken responseObject = JObject.Parse(responseString);
            JArray documentsArray = responseObject.Value<JArray>("data");
            return documentsArray.ToObject<T[]>();
        }
    }
}

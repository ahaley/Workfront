using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ahaley.Workfront
{
    class WorkfrontLogin
    {
        WorkfrontConfiguration config;

        public async Task<bool> Login(WorkfrontConfiguration config)
        {
            this.config = config;
            using (var client = new HttpClient()) {
                client.BaseAddress = new Uri(config.VersionedUrl);
                var resultResponse = await GetApiKey(client);
                if (resultResponse == null || !resultResponse.IsValid) {
                    resultResponse = await GenerateApiKey(client);
                }
                if (resultResponse.IsValid) {
                    config.Token = resultResponse.Result;
                    return true;
                }
                return false;
            }
        }

        async Task<ResultResponse> GetApiKey(HttpClient client)
        {
            string uri = string.Join("?", "user", string.Join("&",
                string.Join("=", "action", "getApiKey"),
                string.Join("=", "username", config.Username),
                string.Join("=", "password", config.Password),
                string.Join("=", "method", "put")));

            HttpResponseMessage response = await client.PutAsync(uri, null);

            if (!response.IsSuccessStatusCode) {
                return null;
            }
            ResultResponse userResponse = await response.ParseScalar<ResultResponse>();
            return userResponse;
        }

        async Task<ResultResponse> GenerateApiKey(HttpClient client)
        {
            string uri = string.Join("?", "user", string.Join("&",
                string.Join("=", "action", "generateApiKey"),
                string.Join("=", "username", config.Username),
                string.Join("=", "password", config.Password),
                string.Join("=", "method", "put")));

            var response = await client.PutAsync(uri, null);

            if (!response.IsSuccessStatusCode) {
                return null;
            }
            ResultResponse resultResponse = await response.ParseScalar<ResultResponse>();
            return resultResponse;
        }

    }
}

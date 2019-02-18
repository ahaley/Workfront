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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(config.VersionedUrl);
                var resultResponse = await GetApiKey(client);
                if (resultResponse == null || !resultResponse.IsValid)
                {
                    resultResponse = await GenerateApiKey(client);
                }
                if (resultResponse.IsValid)
                {
                    config.Token = resultResponse.Result;

                    var sessionResult = await GetSessionID(client);
                    if (sessionResult != null)
                    {
                        config.SessionID = sessionResult.SessionID;
                        return true;
                    }
                }
                return false;
            }
        }

        async Task<LoginResponse> GetSessionID(HttpClient client)
        {
            string uri = string.Join("?", "login", string.Join("&",
                string.Join("=", "username", config.Username),
                string.Join("=", "password", config.Password)));

            var response = await client.PostAsync(uri, null);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var loginResponse = await response.ParseScalar<LoginResponse>();
            return loginResponse;
        }

        async Task<ResultResponse> GetApiKey(HttpClient client)
        {
            string uri = string.Join("?", "user", string.Join("&",
                string.Join("=", "action", "getApiKey"),
                string.Join("=", "username", config.Username),
                string.Join("=", "password", config.Password),
                string.Join("=", "method", "put")));

            HttpResponseMessage response = await client.PutAsync(uri, null);

            if (!response.IsSuccessStatusCode)
            {
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

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            ResultResponse resultResponse = await response.ParseScalar<ResultResponse>();
            return resultResponse;
        }

    }
}

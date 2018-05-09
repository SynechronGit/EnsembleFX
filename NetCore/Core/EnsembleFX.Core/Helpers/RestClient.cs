using EnsembleFX.Core.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace EnsembleFX.Core.Helpers
{
    public class RestClient<Input, Output>
    {
        #region Private Members
        private readonly HttpClient _client;
        private readonly Dictionary<string, string> token;

        #endregion

        #region Public Properties
        public bool Success { get; private set; }
        public string Response { get; private set; }
        public Output Result { get; private set; }

        #endregion

        #region Constructor
        public RestClient(string baseAddress, IOptions<ServiceAccountAppSettings> appSettings, bool byPassAnonymousRequest = false)
        {
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = false;

            _client = new HttpClient(handler);
            _client.BaseAddress = new Uri(baseAddress);
            _client.Timeout = TimeSpan.FromMinutes(10);


            //TODO
            //AS OF NOW WE DON'T HAVE TOKEN SERVICE, SO BELOW CODE IS COMMENTED FOR TESTING PURPOSE.
            //ONCE WE HAVE TOKEN SERVICE THEN WE CAN UNCOMMENT BELOW CODE AND DO THE PROPER TESTING.

            //var postData = new List<KeyValuePair<string, string>>();
            //postData.Add(new KeyValuePair<string, string>
            //                   ("grant_type", "password"));
            //postData.Add(new KeyValuePair<string, string>
            //                   ("username", appSettings.Value.ServiceAccountUserName));
            //postData.Add(new KeyValuePair<string, string>
            //                   ("password", appSettings.Value.ServiceAccountPassword));

            //HttpContent content = new FormUrlEncodedContent(postData);
           
            //if (!byPassAnonymousRequest)
            //{
            //    string result = string.Empty;
            //    try
            //    {
            //        if (TokenInstance.ApiToken == null || string.IsNullOrWhiteSpace(TokenInstance.ApiToken.access_token) || TokenInstance.ApiToken.expires < DateTime.UtcNow)
            //        {
            //            lock (TokenInstance.ApiToken)
            //            {
            //                if (TokenInstance.ApiToken == null || string.IsNullOrWhiteSpace(TokenInstance.ApiToken.access_token) || TokenInstance.ApiToken.expires < DateTime.UtcNow)
            //                {
            //                    result = _client.PostAsync(baseAddress + "/Token", content)
            //                            .Result.Content.ReadAsStringAsync().Result;

            //                    token = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            //                    TokenInstance.ApiToken = this.GetApiToken(token);
            //                }
            //            }
            //        }
            //        Success = false;
            //        _client.DefaultRequestHeaders.Accept.Add(
            //            new MediaTypeWithQualityHeaderValue("application/json"));
            //        var header = new AuthenticationHeaderValue("Bearer", TokenInstance.ApiToken.access_token);
            //        _client.DefaultRequestHeaders.Authorization = header;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(String.Format("Error Message : {0} || Server Response : {1} || Stack Trace : {2}", ex.Message, result, ex.StackTrace));
            //    }
            //}
        }

        #endregion

        #region Public Methods
        public HttpResponseMessage ExecuteGet(string resource)
        {
            return _client.GetAsync(resource).Result;
        }

        public HttpResponseMessage ExecutePost(string resource, Input model)
        {
            var task = Task.Factory.StartNew(() => _client.PostAsJsonAsync(resource, model));
            task.Wait();
            if (task.IsCompleted)
            {
                return task.Result.Result;
            }
            return null;
        }

        public void ExecutePostHttpContent(string resource, Input model)
        {
            var task = Task.Factory.StartNew(() => _client.PostAsync(resource, model as HttpContent));
            task.Wait();
            if (task.IsCompleted)
            {
                HttpResponseMessage response = task.Result.Result;
                prepareResult(response);
            }
        }

        //public HttpResponseMessage ExecutePut(string resource, Input model)
        //{
        //    var task = Task.Factory.StartNew(() => _client.PutAsJsonAsync(resource, model));
        //    task.Wait();
        //    if (task.IsCompleted)
        //    {
        //        return task.Result.Result;
        //    }
        //    return null;
        //}

        public HttpResponseMessage ExecuteDelete(string resource)
        {
            return _client.DeleteAsync(resource).Result;
        }

        public void Dispose()
        {
            if (_client != null)
                _client.Dispose();
        }

        #endregion

        #region Private Method
        private void prepareResult(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                Success = true;
                Response = Convert.ToString(response);
                if (typeof(Output) == typeof(Stream))
                    Result = (Output)(object)(response.Content.ReadAsStreamAsync().Result);
                else
                    Result = (Output)(object)response.Content.ReadAsByteArrayAsync().Result;
            }
            else
            {
                Success = false;
                Result = default(Output);
                Response = Convert.ToString(response);
            }
        }

        private ApiToken GetApiToken(Dictionary<string, string> token)
        {
            var apiToken = new ApiToken();
            apiToken.access_token = token["access_token"];
            //apiToken.IsValidUser = bool.Parse(token["IsValidUser"]);
            apiToken.token_type = token["token_type"];
            apiToken.UserId = token["userId"];
            apiToken.issued = token[".issued"];


            var expireDate = token[".expires"].Split(' ');
            int year = int.Parse(expireDate[3]);
            int month = DateTime.ParseExact(expireDate[2], "MMM", CultureInfo.InvariantCulture).Month;
            int day = int.Parse(expireDate[1]);

            var expireTime = expireDate[4].Split(':');
            int hour = int.Parse(expireTime[0]);
            int minute = int.Parse(expireTime[1]);

            apiToken.expires = new DateTime(year, month, day, hour, minute, 0);

            return apiToken;
        }

        #endregion

        #region Destructor
        //~RestClient()
        //{
        //    _client.Dispose();
        //}
        #endregion
    }
}

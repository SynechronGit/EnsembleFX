using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Core.DataFrame.Transporters
{
    public class HttpTransporter : ITransporter
    {
        public HttpTransporter(HttpTransporterConfiguration configuration)
        { Configuration = configuration; }

        public HttpTransporterConfiguration Configuration { get; set; }


        public Stream Read()
        {
            if (string.IsNullOrWhiteSpace(Configuration.ActionUrl))
                throw new Exception("HttpConnectorConfiguration.ActionUrl is not provided.");

            //TODO: Handle HttpMethod verbs
            string urlAddress = Configuration.ActionUrl;
            MemoryStream memoryStream = new MemoryStream();
            var postData = new Dictionary<string, string>();

            if (Configuration.Method == HttpMethod.Post)
            {
                // For Form Data in request               

                if(Configuration.Contents!=null)
                {
                    foreach (var content in Configuration.Contents)
                    {
                        postData.Add(content.Key, content.Value);
                    }
                }                    

                var postDataString = new FormUrlEncodedContent(postData);

                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    // For Adding Headers
                    if(Configuration.Headers!=null)
                    {
                        foreach (var requestData in Configuration.Headers)
                        {
                            client.DefaultRequestHeaders.Add(requestData.Key, requestData.Value);
                        }
                    }            
                    HttpResponseMessage APIResponse = client.PostAsync(Configuration.ActionUrl, postDataString).Result;
                   
                    bool IsSuccess = APIResponse.IsSuccessStatusCode;
                    if (IsSuccess)
                    {
                        Stream streamData = APIResponse.Content.ReadAsStreamAsync().Result;

                       
                        streamData.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                    }
                }

                byte[] bytes = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(bytes, 0, (int)memoryStream.Length);

                string contentString = Encoding.ASCII.GetString(bytes);
            }



            else
            {
                HttpClientHandler handler = new HttpClientHandler()
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };

                using (var client = new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    if (!string.IsNullOrEmpty(Configuration.Token))
                    {
                        client.DefaultRequestHeaders.Add("Token", Configuration.Token);
                    }

                    if (!string.IsNullOrEmpty(Configuration.ApiKey))
                    {
                        client.DefaultRequestHeaders.Add("x-api-key", Configuration.ApiKey);
                    }

                    if (Configuration.Headers != null)
                    {
                        foreach (var requestData in Configuration.Headers)
                        {
                            client.DefaultRequestHeaders.Add(requestData.Key, requestData.Value);
                        }
                    }

                    HttpResponseMessage APIResponse = client.GetAsync(urlAddress).Result;
                    bool IsSuccess = APIResponse.IsSuccessStatusCode;
                    if (IsSuccess)
                    {
                        Stream streamData = APIResponse.Content.ReadAsStreamAsync().Result;
                        streamData.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                    }
                }



            }


            return memoryStream;
        }

        public void Write(Stream data)
        {
            throw new NotImplementedException();
        }


        #region Not in use
        //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);
        //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        //MemoryStream memoryStream = new MemoryStream();
        //StreamWriter streamWriter = new StreamWriter(memoryStream);

        //StreamReader readStream = null;
        //if (response.StatusCode == HttpStatusCode.OK)
        //{
        //    Stream receiveStream = response.GetResponseStream();


        //    if (response.CharacterSet == null)
        //    {
        //        readStream = new StreamReader(receiveStream);
        //    }
        //    else
        //    {
        //        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
        //    }

        //    streamWriter.Write(readStream.ReadToEnd());
        //    streamWriter.Flush();
        //    memoryStream.Position = 0;

        //    response.Close();
        //    readStream.Close();
        //}
        #endregion
    }
}

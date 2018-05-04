using EnsembleFX.Core.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace EnsembleFX.Core.Helpers
{
    public class UploadBlobHelper
    {
        IOptions<BaseUrlAppSettings> _baseUrlAppSettings;
        IOptions<ServiceAccountAppSettings> _accountAppSettings;
        public UploadBlobHelper(IOptions<BaseUrlAppSettings> baseUrlAppSettings, IOptions<ServiceAccountAppSettings> accountAppSettings)
        {
            _baseUrlAppSettings = baseUrlAppSettings;
            _accountAppSettings = accountAppSettings;
        }
        public List<BlobUploadModel> UploadBlob(Stream stream, string fileName)
        {
            var client = new RestClient<MultipartFormDataContent, List<BlobUploadModel>>(_baseUrlAppSettings.Value.BaseUrl, _accountAppSettings);

            MultipartFormDataContent form = new MultipartFormDataContent();

            var content = new StreamContent(stream);

            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "fileToUpload",
                FileName = fileName
            };

            form.Add(content);

            client.ExecutePostHttpContent("/api/CommonBlob/UploadFile", form);
            var result = client.Result;
            client.Dispose();
            return result;
        }
    }
}

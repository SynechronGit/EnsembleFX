using System.Collections.Generic;
using System;
using System.Net.Http;
using EnsembleFX.Core.Helpers;
using Microsoft.Extensions.Options;
using EnsembleFX.Core.Model;

namespace EnsembleFX.ApiProxy
{
    public class RuntimeApiProxy
    {
        #region Public methods
        
        #region Users
        private string ApiBaseUrl;
        private IOptions<ServiceAccountAppSettings> appSettiings;
        public RuntimeApiProxy(IOptions<ApiBaseUrl> apiBaseUrl,  IOptions<ServiceAccountAppSettings> appSettiings){
               this.ApiBaseUrl = apiBaseUrl.Value.BaseUrl; 
               this.appSettiings = appSettiings;
        }
        public bool HasRoleAccessCheck(string domainAccount, string permissionNames, HttpMethod requestType)
        {
            var client = new RestClient<int, bool>(ApiBaseUrl, appSettiings);
            client.ExecuteGet("/Api/UserApi/HasRoleAccessCheck?domainAccount="+domainAccount+"&permissionNames="+permissionNames+"&requestType="+requestType);
            return client.Result;
        }

        public int GetUserIDByName(string userName)
        {
            var client = new RestClient<string, int>(ApiBaseUrl, appSettiings);
            client.ExecuteGet("/Api/UserApi/GetUserId?userName=" + userName);
            return client.Result;
        }
        #endregion

        public bool IPAddressAllowed(string ipAddress)
        {
            var client = new RestClient<int, bool>(ApiBaseUrl, appSettiings,true);
            client.ExecuteGet("Api/IPFenceApi/IPAddressAllowed?ipAddress=" + ipAddress);
            return client.Result;
        }

        #endregion
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using  Microsoft.AspNetCore.Http;

namespace EnsembleFX.Helper
{
    public static class OperationHelper
    {
        private IHttpContextAccessor httpContextAccessor;
        public OperationHelper(IHttpContextAccessor httpContextAccessor){
            this.httpContextAccessor = httpContextAccessor;
        }

        public static string ReadRequestAsString(object entity)
        {
            return JsonConvert.SerializeObject(entity);
        }


        public static List<TEntity> ConvertDocumentToList<TEntity>(IEnumerable<BsonDocument> documents)
        {
            try
            {
                List<TEntity> entities = new List<TEntity>();

                foreach (var document in documents)
                {
                    TEntity data = BsonSerializer.Deserialize<TEntity>(document);
                    entities.Add(data);
                }
                return entities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// Get logged in Username
        /// </summary>
        /// <returns>logged in Username</returns>
        public static string GetUserName() {
            var HttpContext = this.httpContextAccessor.HttpContext;
            return (HttpContext.Current != null && 
                    HttpContext.Current.User != null &&
                    HttpContext.Current.User.Identity != null && 
                    !string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name)) ? 
                        HttpContext.Current.User.Identity.Name : string.Empty;
        }
    }
}
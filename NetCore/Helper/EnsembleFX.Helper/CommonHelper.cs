using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace EnsembleFX.Helper
{
    public static class CommonHelper
    {
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
    }
}

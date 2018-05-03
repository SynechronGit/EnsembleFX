using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Helper
{
    public interface IOperationHelper
    {       
        string GetUserName();
        string ReadRequestAsString(object entity);

        List<TEntity> ConvertDocumentToList<TEntity>(IEnumerable<BsonDocument> documents);
    }
}

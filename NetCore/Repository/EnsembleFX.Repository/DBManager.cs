using EnsembleFX.Logging;
using EnsembleFX.Repository.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Repository
{
    public class DBManager<TEntity> where TEntity : class
    {
        private IDBRepository<TEntity> _dbRepository;

        public DBManager(string collection, ILogController logController, IOptions<ConnectionStrings> connectionStrings)
        {
            if (!string.IsNullOrEmpty(collection))
            {
                _dbRepository = new MongoDbRepository<TEntity>(collection, logController, connectionStrings);
            }
        }

        public IDBRepository<TEntity> Instance
        {
            get
            {
                return _dbRepository;
            }
        }
    }
}

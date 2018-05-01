using EnsembleFX.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Repository
{
    public class DBManager<TEntity> where TEntity : class
    {
        private IDBRepository<TEntity> _dbRepository;

        public DBManager(string collection, ILogController logController)
        {
            if (!string.IsNullOrEmpty(collection))
            {
                _dbRepository = new MongoDbRepository<TEntity>(collection, logController);
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

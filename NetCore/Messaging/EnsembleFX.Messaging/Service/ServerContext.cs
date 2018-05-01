using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public class ServerContext : Dictionary<string, object>, IServerContext
    {

        internal RoutingTable routingTable;

        #region Public Methods

        public new void Clear()
        {
            base.Clear();
        }
        #endregion

        #region Public Properties
        public string ServerName { get; set; }
        #endregion


        public RoutingTable RoutingTable
        {
            get { return routingTable; }
        }
    }
}

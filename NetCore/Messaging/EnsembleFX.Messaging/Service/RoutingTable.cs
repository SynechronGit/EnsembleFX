using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Messaging.Service
{
    public class RoutingTable
    {
       internal Dictionary<string, List<string>> Routes { get; set; }
       
        public void Clear()
        {
            Routes.Clear();
        }

        public void RegisterRoute(string messageType, string consumer)
        {
            if(!Routes.ContainsKey(messageType))
            {
                Routes.Add(messageType, new List<string>());
            }
            if(!Routes[messageType].Contains(consumer))
            {
                Routes[messageType].Add(consumer);
            }
        }

        public void RegisterRoute(List<string> messageTypes, string consumer)
        {
            foreach(string messageType in messageTypes)
            {
                RegisterRoute(messageType, consumer);
            }

        }

        void RegisterRoute(string messageType, List<string> consumers)
        {
            foreach (string consumer in consumers)
            {
                RegisterRoute(messageType, consumer);
            }
        }


        public void UnregisterAllConsumers(string messageType)
        {
            if (!Routes.ContainsKey(messageType))
                return;

                Routes[messageType].Clear();
        }

        public void UnregisterAllMessageTypes(string consumer)
        {
            foreach(string messageType in Routes.Keys)
            {
                UnregisterRoute(messageType, consumer);
            }

        }


        public void UnregisterRoute(string messageType, string consumer)
        {
            if (!Routes.ContainsKey(messageType))
                return;

            if (!Routes[messageType].Contains(consumer))
                return;

            Routes[messageType].Remove(consumer);
        }

        public IEnumerable<string> GetRoute(string messageType)
        {
            if (!Routes.ContainsKey(messageType))
                return null;

            return Routes[messageType].Select(item => (string)item.Clone());
        }
    }

    
       

}

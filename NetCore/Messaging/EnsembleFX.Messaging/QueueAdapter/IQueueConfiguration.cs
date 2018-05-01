using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.QueueAdapter
{
    public interface IQueueConfiguration
    {
        [XmlAttribute]
        string Name { get; set; }

        [XmlAttribute]
        bool IsControlQueue { get; set; }
    }
}

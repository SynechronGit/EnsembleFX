using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.Serialization
{
    [XmlRoot]
    public interface IMessage
    {
        string Environment { get; set; }
    }
}

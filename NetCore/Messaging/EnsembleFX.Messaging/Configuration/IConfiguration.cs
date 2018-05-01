using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.Configuration
{
    public interface IConfiguration
    {
        [XmlAttribute]
        string Name { get; set; }
    }
}

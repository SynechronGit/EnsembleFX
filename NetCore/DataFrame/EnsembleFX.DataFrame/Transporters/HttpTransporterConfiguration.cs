using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.DataFrame.Transporters
{ 
    public class HttpTransporterConfiguration : ITransporterConfiguration
    {
        [XmlElement]
        public string ResourceName { get; set; }

        [XmlElement]
        public HttpMethod Method { get; set; }       

        [XmlElement]
        public string ActionUrl { get; set; }

        [XmlText]
        public string Body { get; set; }

        [XmlArray("Headers")]
        [XmlArrayItem("Header")]
        public Dictionary<string,string> Headers { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public string Token { get; set; }

        [XmlAttribute]
        public string ApiKey { get; set; }
        
        // For POST      
       

        [XmlArray("Contents")]
        [XmlArrayItem("Content")]
        public Dictionary<string, string> Contents { get; set; }

    }

    [Serializable]
    public enum HttpMethod : int
    {
        Unknown = 0,
        Get = 1,
        Post = 2,
        Put = 3,
        Delete = 4
    }

   
}

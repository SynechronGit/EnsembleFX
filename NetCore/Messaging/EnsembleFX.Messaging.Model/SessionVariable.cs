using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging.Model
{
    [Serializable]
    public class SessionVariable
    {
        [XmlAttribute]
        public Guid _id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public bool IsCollection { get; set; }
        [XmlAttribute]
        public string VariableType { get; set; }
        [XmlElement]
        public string ValueString { get; set; }
        [XmlElement]
        public object ValueObject { get; set; }
        [XmlElement]
        public string Description { get; set; }
    }
}

using Newtonsoft.Json.Schema;

namespace EnsembleFX.Core.Model
{
    public class DataType
    {
        public string ID { get; set; }
        public string BusinessName { get; set; }
        public JsonSchemaType JsonType { get; set; }
        public string SQLType { get; set; }
        public string CLRType { get; set; }
        public string SqlLength { get; set; }

    }
}

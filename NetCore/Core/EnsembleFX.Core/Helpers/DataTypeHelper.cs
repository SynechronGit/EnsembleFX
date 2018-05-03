using EnsembleFX.Core.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System.Collections.Generic;
using System.Linq;

namespace EnsembleFX.Core.Helpers
{
    // Hold Information about Data Type Mapper
    //DataType
    //    ID
    //    Business Name
    //    JSonType
    //    SQLType
    //    CLRType
    //1 SSN Int  varchar(9)  string.

    public static class DataTypeHelper
    {

        static string _data = @"[
                    { 'ID': 1,'BusinessName': 'Text','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    ,{ 'ID': 2,'BusinessName': 'Boolean','JsonType': 8,'SQLType': 'boolean','CLRType': 'bool','SqlLength': '1'}
                    ,{ 'ID': 3,'BusinessName': 'Mutiline Text','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    ,{ 'ID': 4,'BusinessName': 'Number','JsonType': 4,'SQLType': 'int','CLRType': 'int','SqlLength': '4'}
                    , { 'ID': 5,'BusinessName': 'Decimal','JsonType': 2,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 6,'BusinessName': 'Amount','JsonType': 4,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 7,'BusinessName': 'Percentage','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 8,'BusinessName': 'Password','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 9,'BusinessName': 'Email','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 10,'BusinessName': 'SSN','JsonType': 4,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 11,'BusinessName': 'Phone','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 12,'BusinessName': 'US State','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 13,'BusinessName': 'US ZipCode','JsonType': 4,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 14,'BusinessName': 'Country','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 15,'BusinessName': 'Currency','JsonType': 4,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 16,'BusinessName': 'Date','JsonType': 1,'SQLType': 'varchar','CLRType': 'DateTime','SqlLength': '4000'}
                    , { 'ID': 17,'BusinessName': 'DateTime','JsonType': 1,'SQLType': 'varchar','CLRType': 'DateTime','SqlLength': '4000'}
                    , { 'ID': 18,'BusinessName': 'Time','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 19,'BusinessName': 'TimeZone','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 20,'BusinessName': 'Scaled Rating','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 21,'BusinessName': 'List Of Values - Single Select','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 22,'BusinessName': 'List Of Values - Multi Select','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 23,'BusinessName': 'Person','JsonType': 16,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 24,'BusinessName': 'Company','JsonType': 16,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 25,'BusinessName': 'Color','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 26,'BusinessName': 'WebSite URL','JsonType': 1,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 27,'BusinessName': 'NAIC Code','JsonType': 4,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 28,'BusinessName': 'SIC Code','JsonType': 4,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    , { 'ID': 29,'BusinessName': 'ISO Class Code','JsonType': 4,'SQLType': 'varchar','CLRType': 'string','SqlLength': '4000'}
                    ]";

        static List<DataType> _dataTypes = null;

        /// <summary>
        /// 
        /// </summary>
        private static List<DataType> DataTypeCollection
        {
            get
            {
                if (null == _dataTypes)
                {
                    _dataTypes = JsonConvert.DeserializeObject<List<DataType>>(_data);
                }
                return _dataTypes;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataType GetDataType(string name)
        {
            var type = _dataTypes.Where(q => q.BusinessName == name).FirstOrDefault();

            if (null != type)
            {
                return type;
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static JsonSchemaType GetJsonMapper(string name)
        {
            var type = DataTypeCollection.Where(q => q.BusinessName == name).FirstOrDefault();

            if (null != type)
            {
                return type.JsonType;
            }

            return JsonSchemaType.None;
        }

        public static List<DataType> GetAll()
        {
            return DataTypeCollection;
        }

        public static string GetJsonMapperNameById(string ID)
        {
            var type = DataTypeCollection.Where(q => q.ID == ID).FirstOrDefault();
            if (null != type)
            {
                return type.BusinessName;
            }

            return null;
        }
    }    
}

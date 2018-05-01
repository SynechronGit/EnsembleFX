using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnsembleFX.Serialization
{
    public interface ISerializer
    {
        string Serialize(object instance);
        object Deserialize(string serializedInstance, string messageTypeName);
        T Deserialize<T>(Uri locationURL);
        T Deserialize<T>(string serializedObject);
        T Deserialize<T>(System.IO.Stream serializedObject);
    }
}

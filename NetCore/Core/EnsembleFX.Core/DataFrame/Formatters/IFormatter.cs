using System;
using System.Data;
using System.IO;

namespace EnsembleFX.Core.DataFrame.Formatters
{
    public interface IFormatter
    {
        void Initialize(IFormatterConfiguration config);
        Type GetConfigurationType();
        DataSet ReadFromStream(Stream content);
        Stream WriteToStream(DataSet table);
    }
}
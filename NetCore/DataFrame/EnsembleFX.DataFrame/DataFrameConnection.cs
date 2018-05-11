using EnsembleFX.DataFrame.Adapters;
using EnsembleFX.DataFrame.Formatters;
using EnsembleFX.DataFrame.Transporters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace EnsembleFX.DataFrame
{
    public class DataFrameConnection
    {
        public DataSet Read(IFormatter formatter, ITransporter transporter, StreamPipeline pipeline)
        {
            //TODO ExceptionHandling
            return formatter.ReadFromStream(transporter.Read());
        }

        public void Write(IFormatter formatter, ITransporter transporter, StreamPipeline pipeline, DataSet dataSet)
        {
            //TODO ExceptionHandling
            transporter.Write(formatter.WriteToStream(dataSet));
        }

        public void Write(IAdapter adapter, StreamPipeline pipeline, DataSet dataSet)
        {
            //TODO ExceptionHandling
            adapter.Write(dataSet);
        }

        public DataSet Read(IAdapter adapter, StreamPipeline pipeline)
        {
            //TODO ExceptionHandling
            return adapter.Read();
        }

        public Stream Read(ITransporter transporter, StreamPipeline pipeline)
        {
            //TODO ExceptionHandling
            return transporter.Read();
        }

        public void Write(ITransporter transporter, StreamPipeline pipeline, Stream content)
        {
            //TODO ExceptionHandling
            transporter.Write(content);
        }
    }
}

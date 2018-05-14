using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Core.DataFrame.Adapters
{
   public interface IAdapter
    {
        void Initialize(IAdapterConfiguration config);
        Type GetConfigurationType();
        DataSet Read();
        void Write(DataSet dataSet);
    }
}

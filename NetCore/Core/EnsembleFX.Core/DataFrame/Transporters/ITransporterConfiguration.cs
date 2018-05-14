using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Core.DataFrame.Transporters
{
    public interface ITransporterConfiguration
    {
        string Name { get; set; }
    }
}

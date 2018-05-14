using System.IO;

namespace EnsembleFX.Core.DataFrame.Transporters
{
    public interface ITransporter
    {
        Stream Read();
        void Write(Stream data);
    }
}
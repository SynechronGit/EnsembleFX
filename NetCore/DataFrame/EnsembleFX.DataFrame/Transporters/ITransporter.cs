using System.IO;

namespace EnsembleFX.DataFrame.Transporters
{
    public interface ITransporter
    {
        Stream Read();
        void Write(Stream data);
    }
}
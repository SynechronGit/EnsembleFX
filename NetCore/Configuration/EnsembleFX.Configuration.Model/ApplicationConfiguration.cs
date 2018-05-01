
namespace EnsembleFX.Configuration.Model
{
    using System.Collections.Generic;
    public class ApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            Settings = new Dictionary<string, string>();
        }

        public static Dictionary<string, string> Settings;
    }
}

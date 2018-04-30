using System;

namespace EnsembleFX.Core.Configuration
{
   
    public class Configuration
    {
        public static string EnsembleGroupFormatString
        {
            get
            {
                return EnsembleGroup.EnsembleGroupName + "/{0}";
            }
        }
    }
}

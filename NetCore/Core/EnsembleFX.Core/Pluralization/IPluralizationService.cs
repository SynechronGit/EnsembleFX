using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Core.Pluralization
{
    public interface IPluralizationService
    {
        string Pluralize(string name);
        string Singularize(string name);
    }
}

using PluralizationService;
using PluralizationService.English;
using System;
using System.Globalization;

namespace EnsembleFX.Pluralization
{
    public class PluralizationService : IPluralizationService
    {
        private readonly IPluralizationApi api;
        private readonly CultureInfo cultureInfo;

        public PluralizationService()
        {
            var builder = new PluralizationApiBuilder();
            builder.AddEnglishProvider();

            api = builder.Build();
            cultureInfo = new CultureInfo("en-US");
        }
        public string Pluralize(string name)
        {
            return api.Pluralize(name, cultureInfo) ?? name;
        }

        public string Singularize(string name)
        {
            return api.Singularize(name, cultureInfo) ?? name;
        }
    }
}

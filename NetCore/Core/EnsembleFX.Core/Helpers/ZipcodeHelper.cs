using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Core.Helpers
{
    public static class ZipcodeHelper
    {
        private const string NLCountryCode = "NL";
        public static string GetFormattedZipCode(string countryCode, string zipcode)
        {
            string formattedZipCode = string.Empty;
            if (countryCode == NLCountryCode)
            {
                formattedZipCode = zipcode.Substring(0, 4).Trim();
            }
            else
            {
                formattedZipCode = zipcode.Trim();
            }
            return formattedZipCode;
        }
    }
}

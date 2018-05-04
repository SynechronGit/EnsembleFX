using System;

namespace EnsembleFX.Core.Model
{
    public class ApiToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }

        public string UserId { get; set; }

        public DateTime expires { get; set; }

        public string issued { get; set; }

        public bool IsValidUser { get; set; }
    }
}

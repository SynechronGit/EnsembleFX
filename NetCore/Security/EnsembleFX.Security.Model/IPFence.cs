using System;

namespace EnsembleFX.Core.Security.Model
{
    public class IPFence
    {
        public Guid _id { get; set; }
        public int Ordinal { get; set; }
        public string Name { get; set; }
        public string WhitelistOrBlacklist { get; set; } //W for Whitelist and B for Blacklist
        public string IPEntryTypeASR { get; set; } //A - IP Address, S is Subnet, R is Range

        public string IPAddress { get; set; } //IP Address for A and S, StartIPAddress for Range

        public string Subnet { get; set; }
        public string EndIPAddress { get; set; }

        public DateTime? StartOnUTC { get; set; }
        public DateTime? EndOnUTC { get; set; }
    }
}

using EnsembleFX.Core.Security.Model;
using System.Collections.Generic;
using System.Linq;

namespace EnsembleFX.Core.Security
{
	/// <summary>
	/// List datastructure for IPFence
	/// </summary>
	public class IPFenceList : List<IPFence>
    {

        /// <summary>
        /// Adds IPFence to list
        /// </summary>
        /// <param name="record">IPFence to add</param>
        public void Add(IPFenceViewModel record)
        {
            this.Add(new IPFence(record));
        }

        /// <summary>
        /// Adds IPFences to list
        /// </summary>
        /// <param name="record">IPFences to add</param>
        public void AddRange(IEnumerable<IPFenceViewModel> records)
        {
            foreach (IPFenceViewModel record in records)
            {
                this.Add(new IPFence(record));
            }
        }

        /// <summary>
        /// Checks whether IPFence belongs to White list and is valid
        /// </summary>
        public bool HasWhitelist()
        {
            return (this.Where(i => i.IsWhitelist() && i.IsValidNow()).Count() > 0);
        }


        /// <summary>
        /// Checks whether IPFence belongs to Black list and is valid
        /// </summary>
        public bool HasBlacklist()
        {
            return (this.Where(i => i.IsBlacklist() && i.IsValidNow()).Count() > 0);
        }

        /// <summary>
        /// Checks whether white list contains passed IP address string
        /// </summary>
        /// <param name="addressToValidate">IP address to be checked</param>
        /// <returns>Boolean response indicating presence or absence of IP address in White List</returns>
        public bool WhitelistContains(System.Net.IPAddress addressToValidate)
        {
            foreach (IPFence fence in this.Where(i => i.IsWhitelist() && i.IsValidNow()))
            {
                if (fence.Contains(addressToValidate))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether black list contains passed IP address string
        /// </summary>
        /// <param name="addressToValidate">IP address to be checked</param>
        /// <returns>Boolean response indicating presence or absence of IP address in Black List</returns>
        public bool BlacklistContains(System.Net.IPAddress addressToValidate)
        {
            foreach (IPFence fence in this.Where(i => i.IsBlacklist() && i.IsValidNow()))
            {
                if (fence.Contains(addressToValidate))
                    return true;
            }
            return false;
        }


        /// <summary>
        /// Validates an IP Address against the IPFencing configuration. IP should be in the Whitelist and NOT in Blacklist for an IP Address to be valid/allowed.
        /// </summary>
        /// <param name="addressToValidate">IP Address to Validate</param>
        /// <returns>Return TRUE if the IP Address should be allowed to access the system based on the configuration.</returns>
        public bool Validate(System.Net.IPAddress addressToValidate)
        {
            bool isInWhitelist = false;
            bool isInBlacklist = false;
            if (HasWhitelist() && WhitelistContains(addressToValidate))
            {
                    isInWhitelist = true;
            }
           
            if (HasBlacklist() && BlacklistContains(addressToValidate))
            {
                isInBlacklist = true;
            }

            return isInWhitelist && !isInBlacklist;
        }
    }
}

using EnsembleFX.Core.Security.Model;
using NetTools;
using System;
using System.Linq;

namespace EnsembleFX.Core.Security
{
    /// <summary>
    /// Datastructure for IPFence View Model
    /// </summary>
    public class IPFence
    {
        #region Public Properties
        protected WhitelistOrBlacklist entryWhiteOrBlack;
        public WhitelistOrBlacklist EntryWhiteOrBlack
        {
            get { return entryWhiteOrBlack; }
        }

        protected IPFenceEntryType entryType;
        public IPFenceEntryType EntryType
        {
            get { return entryType; }
        }

        protected System.Net.IPAddress address;
        public System.Net.IPAddress Address { get { return address; } }

        protected IPAddressRange range;
        public IPAddressRange Range { get { return range; } }

        public DateTime StartOnUTC { get; set; }
        public DateTime EndOnUTC { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for IPFence datastructure
        /// </summary>
        /// <param name="iPFenceViewModel"></param>
        public IPFence(IPFenceViewModel iPFenceViewModel)
        {

            char entryTypeChar = iPFenceViewModel.IPEntryTypeASR.Trim().ToUpper().FirstOrDefault();

            switch (entryTypeChar)
            {
                case 'A':
                    entryType = IPFenceEntryType.IPAddress;
                    break;
                case 'S':
                    entryType = IPFenceEntryType.IPSubnet;
                    break;
                case 'R':
                    entryType = IPFenceEntryType.IPRange;
                    break;
                default:
                    entryType = IPFenceEntryType.IPAddress;
                    break;
            }


            if (iPFenceViewModel.WhitelistOrBlacklist.Trim().ToUpper().FirstOrDefault() == 'W')
                entryWhiteOrBlack = WhitelistOrBlacklist.Whitelist;
            else
                entryWhiteOrBlack = WhitelistOrBlacklist.Blacklist;


            if (iPFenceViewModel.StartOnUTC.HasValue)
                StartOnUTC = iPFenceViewModel.StartOnUTC.Value;
            else
                StartOnUTC = DateTime.MinValue;

            if (iPFenceViewModel.EndOnUTC.HasValue)
                EndOnUTC = iPFenceViewModel.EndOnUTC.Value;
            else
                EndOnUTC = DateTime.MaxValue;

            switch (this.EntryType)
            {
                case IPFenceEntryType.IPAddress:
                    address = System.Net.IPAddress.Parse(iPFenceViewModel.IPAddress);
                    break;
                case IPFenceEntryType.IPRange:
                    range = IPAddressRange.Parse(iPFenceViewModel.IPAddress + "-" + iPFenceViewModel.EndIPAddress);
                    break;
                case IPFenceEntryType.IPSubnet:
                    range = IPAddressRange.Parse(iPFenceViewModel.Subnet);
                    break;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Checks whether this(IPFence) entry belongs to white list 
        /// </summary>
        /// <returns>Boolean response indicating presence or absence</returns>
        public bool IsWhitelist()
        {
            return entryWhiteOrBlack == WhitelistOrBlacklist.Whitelist;
        }

        /// <summary>
        /// Checks whether this(IPFence) entry belongs to black list 
        /// </summary>
        /// <returns>Boolean response indicating presence or absence</returns>
        public bool IsBlacklist()
        {
            return entryWhiteOrBlack == WhitelistOrBlacklist.Blacklist;
        }

        /// <summary>
        /// Checks whether IPFence is valid for current date or not 
        /// </summary>
        /// <returns>Boolean response indicating valid or invalid</returns>
        public bool IsValidNow()
        {
            if (DateTime.UtcNow > StartOnUTC && DateTime.UtcNow < EndOnUTC)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Checks whether string is valid ip address syntactically or not
        /// </summary>
        /// <param name="addressToValidateAsString">Ip address string to validate</param>
        /// <returns>Boolean response indicating valid or invalid</returns>
        public bool Contains(string addressToValidateAsString)
        {
            return Contains(System.Net.IPAddress.Parse(addressToValidateAsString));
        }

        /// <summary>
        /// Checks validate string as valid ip address/Subnet mask/ Range 
        /// </summary>
        /// <param name="addressToValidate">Address string to validate</param>
        /// <returns>Boolean response indicating valid or invalid</returns>
        public bool Contains(System.Net.IPAddress addressToValidate)
        {
            if (!IsValidNow())
                return true;

            switch (entryType)
            {
                case IPFenceEntryType.IPAddress:
                    return address.Equals(addressToValidate);
                case IPFenceEntryType.IPRange:
                    return range.Contains(addressToValidate);
                case IPFenceEntryType.IPSubnet:
                    return range.Contains(addressToValidate);
            }
            return false;
        }
        #endregion
    }
}

namespace EnsembleFX.Core.Security.Model
{
	/// <summary>
	/// Enum for IPAddress,Subnet Mask and IP address range identification
	/// </summary>
	public enum IPFenceEntryType : int
    {
        IPAddress = 0,
        IPSubnet = 1,
        IPRange = 2
    }
}

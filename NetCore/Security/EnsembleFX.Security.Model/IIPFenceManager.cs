using EnsembleFX.Filters;
using System;
using System.Collections.Generic;

namespace EnsembleFX.Core.Security.Model
{
	/// <summary>
	/// Interface for IPFence
	/// </summary>
	public interface IIPFenceManager
    {
        void Init(IEnumerable<IPFenceViewModel> records);
        IList<IPFence> Get();
        IPFenceListViewModel GetWhitelist(PagingFiltering pagingFiltering);
        IPFenceListViewModel GetBlacklist(PagingFiltering pagingFiltering);
        IPFence Get(Guid ipFenceId);
        IPFence Add(IPFence IPFence);
        IPFence Update(IPFence IPFence);
        bool Delete(Guid ipFenceId);
        bool Validate(string ipAddress, IEnumerable<IPFenceViewModel> records);
    }
}
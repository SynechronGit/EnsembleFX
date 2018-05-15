using EnsembleFX.Communication.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Communication.Abstractions
{
    /// <summary>
	/// Wrapper interface for processing and sending messages depending on implementation. There can be Email messages and SMS messages
	/// </summary>
	public interface IMessageTransportProvider
    {
        /// <summary>
        /// Asychronously process message and send it
        /// </summary>
        /// <param name="transportMessage">Message object with details for sending </param>
        /// <returns></returns>
        Task<bool> SendMessageAsync(TransportMessage transportMessage);
    }
}

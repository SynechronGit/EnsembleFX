using EnsembleFX.Communication.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Communication.Abstractions
{
    /// <summary>
    /// An interface which will be called from service for sending different types of messages (Email or SMS)
    /// </summary>
    public interface ICommunicator
    {
        /// <summary>
        /// Send an Email or SMS message
        /// </summary>
        /// <param name="transportMessage">Message object with details for sending</param>
        /// <returns></returns>
        Task<bool> SendAsync(TransportMessage transportMessage);
    }
}

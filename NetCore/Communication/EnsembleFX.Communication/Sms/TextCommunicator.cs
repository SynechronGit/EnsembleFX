using EnsembleFX.Communication.Abstractions;
using EnsembleFX.Communication.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnsembleFX.Communication.Sms
{
    /// <summary>
	/// Class assigned for processing only SMS text messages
	/// </summary>
	public class TextCommunicator : ICommunicator
    {
        #region Private memebers

        private readonly IConfiguration configuration;
        private readonly IList<IMessageTransportProvider> messageTransportProviders;

        #endregion

        #region Constructors

        public TextCommunicator(IConfiguration configuration, List<IMessageTransportProvider> messageTransportProviders)
        {
            this.configuration = configuration;
            this.messageTransportProviders = messageTransportProviders;
        }

        #endregion

        #region ICommunicator Implementation

        /// <summary>
        /// Send SMS message
        /// </summary>
        /// <param name="transportMessage">Message object with details for sending</param>
        /// <returns></returns>
        public async Task<bool> SendAsync(TransportMessage transportMessage)
        {
            var status = true;
            try
            {
                foreach (IMessageTransportProvider provider in messageTransportProviders)
                {
                    if (!await provider.SendMessageAsync(transportMessage))
                    {
                        //TODO : Log error
                        status = false;
                    }
                }
            }
            catch (Exception e)
            {
                //TODO : Log exception
                status = false;
            }

            return status;
        }

        #endregion
    }
}

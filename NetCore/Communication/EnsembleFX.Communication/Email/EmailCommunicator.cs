using EnsembleFX.Communication.Abstractions;
using EnsembleFX.Communication.Model;
using EnsembleFX.Communication.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnsembleFX.Communication.Email
{
    /// <summary>
	/// Class assigned for processing Email messages
	/// </summary>
	public class EmailCommunicator : ICommunicator
    {
        #region Private memebers

        private readonly IOptions<EmailAppSettings> emailAppSettings;
        //private readonly IList<IMessageTransportProvider> messageTransportProviders;
        private readonly IMessageTransportProvider messageTransportProviders;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailCommunicator"/> class.
        /// </summary>
        /// <param name="configuration">Configuration which is going to be used for Email communicator</param>
        /// <param name="messageTransportProviders">IEmailTransportProvider can contain only AzureSendGridEmailTransportProvider 
        /// or SMTPMailTransportProvider or both based on configuration. </param>
        public EmailCommunicator(IOptions<EmailAppSettings> emailAppSettings, IMessageTransportProvider messageTransportProviders)
        {
            this.emailAppSettings = emailAppSettings;
            this.messageTransportProviders = messageTransportProviders;
        }

        #endregion

        #region ICommunicator Implementation

        /// <summary>
        /// Send Email message with Azure SendGrid or MailKit client
        /// </summary>
        /// <param name="transportMessage">Message object with details for sending</param>
        /// <returns><c>True</c> if every message from list has been sent succesfully, else it will return <c>False</c>. 
        /// Note that every message will be sent regardless if some messages before were not sent. </returns>
        public async Task<bool> SendAsync(TransportMessage transportMessage)
        {
            var status = true;
            try
            {
                //foreach (IMessageTransportProvider provider in messageTransportProviders)
                //{
                //    if (!await provider.SendMessageAsync(transportMessage))
                //    {
                //        //TODO : Log error
                //        status = false;
                //    }
                //}
                
                    if (!await messageTransportProviders.SendMessageAsync(transportMessage))
                    {
                        //TODO : Log error
                        status = false;
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

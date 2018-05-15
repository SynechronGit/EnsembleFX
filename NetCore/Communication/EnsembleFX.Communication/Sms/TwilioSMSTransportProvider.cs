using EnsembleFX.Communication.Abstractions;
using EnsembleFX.Communication.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace EnsembleFX.Communication.Sms
{
    /// <summary>
	/// Sending SMS message using Twilio client
	/// </summary>
	public class TwilioSMSTransportProvider : IMessageTransportProvider
    {
        #region Private members

        private readonly IConfiguration configuration;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TwilioSMSTransportProvider"/> class.
        /// </summary>
        /// <param name="configuration">Configuration which is going to be used for Twilio</param>
        public TwilioSMSTransportProvider(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        #endregion

        #region IMessageTransportProvider Implementation

        /// <summary>
        /// Asychronously process message and send it
        /// </summary>
        /// <param name="transportMessage">Message object with details for sending </param>
        /// <returns></returns>
        public async Task<bool> SendMessageAsync(TransportMessage transportMessage)
        {
            if (transportMessage == null)
            {
                throw new ArgumentNullException("Message can not be null.");
            }

            try
            {
                var processedMessage = this.Process(transportMessage);

                var result = await Task.FromResult<MessageResource>(MessageResource.Create(
                    to: processedMessage.To,
                    from: processedMessage.From,
                    body: processedMessage.Body));

                if (result.Status.Equals(MessageResource.StatusEnum.Failed) || result.Status.Equals(MessageResource.StatusEnum.Undelivered))
                {
                    //TODO : Log error
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                //TODO : Log exception
                return false;
            }
        }

        #endregion

        #region Private methods

        // Process message for our purpose and return processed message
        private TransportMessage Process(TransportMessage transportMessage)
        {
            //TODO : Template needs to be decided and other message processing
            var processedMessage = transportMessage;

            return processedMessage;
        }

        // Gets a configuration from source using key value
        private string GetConfiguration(string key)
        {
            return this.configuration[key];
        }

        #endregion
    }
}

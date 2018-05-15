using EnsembleFX.Communication.Abstractions;
using EnsembleFX.Communication.Models;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Net;
using System.Threading.Tasks;

namespace EnsembleFX.Communication.Email
{
    public class AzureSendGridMailTransportProvider : IMessageTransportProvider
    {
        #region Private members

        private IConfiguration configuration;
        private ISendGridClient sendGridClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureSendGridMailTransportProvider"/> class.
        /// </summary>
        /// <param name="configuration">Configuration which is going to be used for Azure SendGrid</param>
        /// <param name="sendGridClient">Client which has methods for using SendGrid functions</param>
        public AzureSendGridMailTransportProvider(IConfiguration configuration, ISendGridClient sendGridClient)
        {
            this.configuration = configuration;
            this.sendGridClient = sendGridClient;

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
                var sendGridMessage = this.CreateSendGridMessage(processedMessage);

                var responseStatus = await this.sendGridClient.SendEmailAsync(sendGridMessage);
                if (responseStatus.StatusCode.Equals(HttpStatusCode.OK) || responseStatus.StatusCode.Equals(HttpStatusCode.Accepted))
                {
                    //TODO : Log success with responseStatus.DeserializeResponseBody(responseStatus.Body);
                    return true;
                }
                else
                {
                    //TODO : Log error with responseStatus.DeserializeResponseBody(responseStatus.Body);
                    return false;
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

        private SendGridMessage CreateSendGridMessage(TransportMessage message)
        {
            var sendGridMessage = new SendGridMessage();
            sendGridMessage.From = new EmailAddress(message.From);
            sendGridMessage.AddTo(new EmailAddress(message.To));
            sendGridMessage.Subject = message.Subject;
            sendGridMessage.AddCc(message.CC);
            sendGridMessage.AddBcc(message.Bcc);
            sendGridMessage.PlainTextContent = message.Body;

            return sendGridMessage;
        }

        #endregion
    }
}

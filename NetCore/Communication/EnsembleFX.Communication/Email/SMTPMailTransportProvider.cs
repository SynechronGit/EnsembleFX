using EnsembleFX.Communication.Abstractions;
using EnsembleFX.Communication.Models;
using MailKit;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace EnsembleFX.Communication.Email
{
    /// <summary>
	/// Sending Email message using MailKit client
	/// </summary>
	public class SMTPMailTransportProvider : IMessageTransportProvider
    {
        #region Private members

        private readonly IConfiguration configuration;
        private IMailTransport mailTransport;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SMTPMailTransportProvider"/> class.
        /// </summary>
        /// <param name="configuration">Configuration which is going to be used for SMTP Mail</param>
        public SMTPMailTransportProvider(IConfiguration configuration, IMailTransport mailTransport)
        {
            this.configuration = configuration;
            this.mailTransport = mailTransport;
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
                var mimeMessage = this.CreateMessage(processedMessage);

                await this.mailTransport.SendAsync(mimeMessage);
                return true;
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

        //Create MimeMessage using TransportMessage object
        private MimeMessage CreateMessage(TransportMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("", message.From));
            mimeMessage.To.Add(new MailboxAddress("", message.To));
            mimeMessage.Subject = message.Subject;
            mimeMessage.Cc.Add(InternetAddress.Parse(message.CC));
            mimeMessage.Bcc.Add(InternetAddress.Parse(message.Bcc));
            mimeMessage.Body = new TextPart("plain")
            {
                Text = message.Body
            };

            return mimeMessage;
        }
        #endregion
    }
}

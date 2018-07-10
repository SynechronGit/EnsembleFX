using EnsembleFX.Communication.Abstractions;
using EnsembleFX.Communication.Model;
using EnsembleFX.Communication.Models;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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

        private readonly IOptions<EmailAppSettings> emailAppSettings;
        private IMailTransport mailTransport;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SMTPMailTransportProvider"/> class.
        /// </summary>
        /// <param name="configuration">Configuration which is going to be used for SMTP Mail</param>
        public SMTPMailTransportProvider(IOptions<EmailAppSettings> emailAppSettings, IMailTransport mailTransport)
        {
            this.emailAppSettings = emailAppSettings;
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

                this.mailTransport.Connect(this.emailAppSettings.Value.Host, this.emailAppSettings.Value.Port);
                if (this.mailTransport.IsConnected)
                {
                    this.mailTransport.Authenticate(this.emailAppSettings.Value.Username, this.emailAppSettings.Value.Password);
                    if (this.mailTransport.IsAuthenticated)
                    {
                        await this.mailTransport.SendAsync(mimeMessage);
                        return true;
                    }
                }
                return false;
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
        //private string GetConfiguration(string key)
        //{
        //    return this.configuration[key];
        //}

        //Create MimeMessage using TransportMessage object
        private MimeMessage CreateMessage(TransportMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("", message.From));
            mimeMessage.To.Add(new MailboxAddress("", message.To));
            if (!string.IsNullOrWhiteSpace(message.CC))
            {
                mimeMessage.Cc.Add(new MailboxAddress("", message.CC));
            }
            if (!string.IsNullOrWhiteSpace(message.Bcc))
            {
                mimeMessage.Bcc.Add(new MailboxAddress("", message.Bcc));
            }
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart("plain")
            {
                Text = message.Body
            };

            return mimeMessage;
        }
        #endregion
    }
}

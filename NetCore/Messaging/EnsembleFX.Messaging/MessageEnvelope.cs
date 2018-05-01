using EnsembleFX.Messaging.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EnsembleFX.Messaging
{
    [Serializable]
    [XmlRoot]
    public class MessageEnvelope : IMessageEnvelope
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEnvelope"/> class.
        /// </summary>
        public MessageEnvelope()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageEnvelope"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MessageEnvelope(IMessage message)
            : this()
        {
            Message = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the message UID.
        /// </summary>
        /// <value>The message UID.</value>
        [XmlAttribute]
        public Guid MessageUID { get; set; }

        /// <summary>
        /// Gets or sets the topic.
        /// </summary>
        /// <value>The topic.</value>
        [XmlAttribute]
        public string Topic { get; set; }

        /// <summary>
        /// Gets or sets the message sent on.
        /// </summary>
        /// <value>The message sent on.</value>
        [XmlAttribute]
        public DateTime MessageSentOn { get; set; }

        /// <summary>
        /// Gets or sets the originator.
        /// </summary>
        /// <value>The originator.</value>
        [XmlAttribute]
        public string Originator { get; set; }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        [XmlAttribute]
        public string User { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        /// <value>The reply to.</value>
        [XmlAttribute]
        public string ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the reply of.
        /// </summary>
        /// <value>The reply of.</value>
        [XmlAttribute]
        public Guid ReplyOf { get; set; }

        /// <summary>
        /// Gets or sets the expires on.
        /// </summary>
        /// <value>The expires on.</value>
        [XmlAttribute]
        public DateTime ExpiresOn { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [XmlIgnore]
        public IMessage Message { get; set; }

        /// <summary>
        /// Gets or sets the type of the message.
        /// </summary>
        /// <value>The type of the message.</value>
        [XmlAttribute]
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the message string.
        /// </summary>
        /// <value>The message string.</value>
        [XmlElement]
        public string MessageString { get; set; }

        [XmlElement]
        public string Environment { get; set; }


        [XmlElement]
        public string AllowedEnvironments { get; set; }

        #endregion
    }
}

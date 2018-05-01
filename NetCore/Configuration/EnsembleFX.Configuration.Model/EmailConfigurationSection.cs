
namespace EnsembleFX.Configuration.Model
{
    using System.Configuration;
    public sealed class EmailConfigurationSection : ConfigurationSection
    {
        #region Constants

        internal const string EmailSectionName = "ensemble.Email";

        #endregion

        #region Public Properties

        [ConfigurationProperty("sendEMail", DefaultValue = false, IsRequired = true)]
        public bool SendEMail
        {
            get
            {
                return (bool)base["sendEMail"];
            }
        }

        [ConfigurationProperty("blackList")]
        public BlackListElement BlackList
        {
            get
            {
                return (BlackListElement)base["blackList"];
            }
        }

        [ConfigurationProperty("whiteList")]
        public WhiteListElement WhiteList
        {
            get
            {
                return (WhiteListElement)base["whiteList"];
            }
        }

        [ConfigurationProperty("message")]
        public MessageElement Message
        {
            get
            {
                return (MessageElement)base["message"];
            }
        }

        #endregion

        public static EmailConfigurationSection GetCurrent()
        {
            //coreVelocity/ensemble.Email
            return (EmailConfigurationSection)ConfigurationManager.GetSection(@"ensemble/" + EmailSectionName);
        }

    }
}

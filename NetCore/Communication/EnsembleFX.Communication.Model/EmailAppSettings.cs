using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Communication.Model
{
    public class EmailAppSettings
    {
        /// <summary>
        /// This will be Host Url address to connect to Email Server
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// This will be Port number to connect to Email Server
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// This will be used to authenticate Email Server
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// This will be used to authenticate Email Server
        /// </summary>
        public string Password { get; set; }
    }
}

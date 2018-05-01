using System;
using System.Collections.Generic;
using System.Text;

namespace EnsembleFX.Communication.Model
{
    class TransportMessage
	{
		#region Public properties
		/// <summary>
		/// Blind carbon copy of email message
		/// </summary>
		public string Bcc { get; set; }

		/// <summary>
		/// Content of the message
		/// </summary>
		public string Body { get; set; }

		/// <summary>
		/// Carbon copy of email message
		/// </summary>
		public string CC { get; set; }

		/// <summary>
		/// Sender of the message
		/// </summary>
		public string From { get; set; }

		/// <summary>
		/// Subject of the message
		/// </summary>
		public string Subject { get; set; }

		/// <summary>
		/// Message recipient
		/// </summary>
		public string To { get; set; }

		#endregion
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HelpDeskTickets.ApplicationLogic.Entities.AppSettings
{
	public class SmtpOptions
	{
		public string smtpUserName { get; set; }
		public string smtpPassword { get; set; }
		public string smtpHost { get; set; }
		public int smtpPort { get; set; }
		public bool smtpSSL { get; set; }
		public string fromEmail { get; set; }
		public string fromFullName { get; set; }
	}
}

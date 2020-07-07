using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using HelpDeskTickets.ApplicationLogic.Entities.AppSettings;
using HelpDeskTickets.ApplicationLogic.Interfaces;
using System.Threading.Tasks;

namespace HelpDeskTickets.ApplicationLogic.Services
{
    /// <summary>
    /// Email Sender
    /// </summary>
	public class EmailSender : IEmailSender
	{
		private readonly IOptions<SendGridOptions> _sendGridOptions;

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="sendGridOptions"></param>
		public EmailSender(IOptions<SendGridOptions> sendGridOptions)
		{
			_sendGridOptions = sendGridOptions;
		}

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
		public async Task SendEmailAsync(string email, string subject, string message)
		{
			var client = new SendGridClient(_sendGridOptions.Value.SendGridKey);
			var to = new EmailAddress(email);
			var from = new EmailAddress(_sendGridOptions.Value.FromEmail, _sendGridOptions.Value.FromFullName);
			var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
			var response = await client.SendEmailAsync(msg);

			// Wrap in try catch
		}
	}
}
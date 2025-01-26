using Microsoft.Extensions.Logging;
using Skinet_Store.Core.Entities;
using Skinet_Store.Infrastructure.EmailSenderUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skinet_Store.Infrastructure.EmailSenderUtility
{
	public class EmailSender : IEmailSender, IEmailSender<ApplicationUser>

	{
		private readonly ILogger _logger;

		public EmailSender(ILogger<EmailSender> logger)
		{
			_logger = logger;
		}
		private List<Email> Emails { get; set; } = new();

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			_logger.LogWarning($"{email} {subject} {htmlMessage}");
			Emails.Add(new(email, subject, htmlMessage));
			return Task.CompletedTask;
		}

		Task IEmailSender<ApplicationUser>.SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
		{
			return Task.CompletedTask;
		}

		Task IEmailSender<ApplicationUser>.SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode)
		{
			return Task.CompletedTask;
		}

		Task IEmailSender<ApplicationUser>.SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink)
		{
			return Task.CompletedTask;
		}

	}

	sealed record Email(string Address, string Subject, string HtmlMessage);
}


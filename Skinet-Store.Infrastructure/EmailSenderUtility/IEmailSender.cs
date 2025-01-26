namespace Skinet_Store.Infrastructure.EmailSenderUtility
{
	
		public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string htmlMessage);
	}

	public interface IEmailSender<TUser>
	{
		Task SendConfirmationLinkAsync(TUser user, string email, string confirmationLink);
		Task SendPasswordResetCodeAsync(TUser user, string email, string resetCode);
		Task SendPasswordResetLinkAsync(TUser user, string email, string resetLink);
	}
	

}
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdentityStandaloneMfa.Common
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }

    public class AppSettings
    {
        public string Secret { get; set; }
        public string DoYouwantsTorunMigration { get; set; }
    }
}

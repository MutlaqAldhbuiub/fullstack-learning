using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AuthService.Models;

namespace AuthService.Data
{
    // Implementing the Identity IEmailSender interface
    public class NullEmailSender : IEmailSender<User>
    {
        // This method doesn't do anything since we are not sending emails
        public Task SendEmailAsync(User user, string subject, string message)
        {
            // No-op: Do nothing
            return Task.CompletedTask;
        }
    }
}

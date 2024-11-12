using System.Threading.Tasks;
using AuthService.Models;
namespace AuthService.Data
{
    public interface IEmailSender<User>
    {
        Task SendEmailAsync(User user, string subject, string message);
    }
}

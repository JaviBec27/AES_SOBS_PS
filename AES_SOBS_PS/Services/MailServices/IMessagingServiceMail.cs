using System.Threading.Tasks;

namespace AES_SOBS_PS.Services.MailServices
{
    public interface IMessagingServiceMail
    {
        Task SendEmailAsync(
            string toName,
            string toemailAddress,
            string subject,
            string message,
            params string[] attachments
            );
    }
}

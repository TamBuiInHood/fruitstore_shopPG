using Shared.Services.Email;

namespace Contracts.Services
{
    public interface ISmtpEmailServices : IEmailServices<MailRequest>
    {
    }
}

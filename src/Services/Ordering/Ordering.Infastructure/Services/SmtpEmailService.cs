using Contracts.Configurations;
using Contracts.Services;
using Infrastructure.Configurations;
using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Ocsp;
using Serilog;
using Shared.Services.Email;

namespace Ordering.Infastructure.Services
{
    public class SmtpEmailService : ISmtpEmailServices
    {
        private readonly ILogger _logger;
        private readonly SMTPEmailSetting _settings;
        private readonly SmtpClient _smtpClient;
        public SmtpEmailService(ILogger logger, SMTPEmailSetting settings)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _smtpClient = new SmtpClient();
        }

        public async Task SendEmailAsync(MailRequest mailRequest, CancellationToken cancellationToken = default)
        {
            var emailMessage = new MimeMessage()
            {
                Sender = new MailboxAddress(_settings.DisplayName, mailRequest.From ?? _settings.From),
                Subject = mailRequest.Subject,
                Body = new BodyBuilder()
                {
                    HtmlBody = mailRequest.Body

                }.ToMessageBody()
            };

            if (mailRequest.ToAddresses.Any())
            {
                foreach (var toAddress in mailRequest.ToAddresses)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(toAddress));
                }
            } else
            {
                var toAddress = mailRequest.ToAddress;
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }

            try
            {
                await _smtpClient.ConnectAsync(_settings.StmpServer, _settings.Port, _settings.UseSsl, cancellationToken);
                await _smtpClient.AuthenticateAsync(_settings.UserName, _settings.Password, cancellationToken);
                await _smtpClient.SendAsync(emailMessage, cancellationToken);
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
                _smtpClient.Dispose();
            }
        }
    }
}

namespace Contracts.Services
{
    public interface IEmailServices<T> where T : class
    {
        Task SendEmailAsync(T mailRequest, CancellationToken cancellationToken = new CancellationToken());
    }
}

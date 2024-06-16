using TucaAPI.src.Dtos.Mail;

namespace TucaAPI.src.Providers
{
    public interface IMailSenderProvider
    {
        Task<bool> SendAsync<T>(T mailData)
            where T : BaseMailData;
        Task<bool> SendHtmlAsync<T>(T mailData)
            where T : BaseHtmlMailData;
    }
}

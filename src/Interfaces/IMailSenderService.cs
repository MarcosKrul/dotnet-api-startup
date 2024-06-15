using TucaAPI.src.Dtos.Mail;

namespace TucaAPI.src.Interfaces
{
    public interface IMailSenderService
    {
        Task<bool> SendAsync<T>(T mailData) where T : BaseMailData;
        Task<bool> SendHtmlAsync<T>(T mailData) where T : BaseHtmlMailData;
    }
}
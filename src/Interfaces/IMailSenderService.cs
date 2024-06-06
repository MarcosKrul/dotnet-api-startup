using TucaAPI.src.Dtos.Mail;

namespace TucaAPI.src.Interfaces
{
    public interface IMailSenderService
    {
        Task<bool> SendAsync<T>(T mailData) where T : BaseMailData;
    }
}
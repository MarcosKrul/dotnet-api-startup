namespace TucaAPI.src.Dtos.Mail
{
    public class BaseMailData
    {
        public string EmailToId { get; set; } = string.Empty;
        public string EmailToName { get; set; } = string.Empty;
        public string EmailSubject { get; set; } = string.Empty;
        public string EmailBody { get; set; } = string.Empty;
    }
}

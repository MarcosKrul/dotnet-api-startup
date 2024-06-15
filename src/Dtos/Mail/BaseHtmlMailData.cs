
namespace TucaAPI.src.Dtos.Mail
{
    public class BaseHtmlMailData : BaseMailData
    {
        public string Template { get; set; } = string.Empty;
        public object[]? Args { get; set; }
    }
}
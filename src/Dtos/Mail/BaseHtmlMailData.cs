namespace TucaAPI.src.Dtos.Mail
{
    public class BaseHtmlMailData : BaseMailData
    {
        public StringWriter TemplateWriter { get; set; } = new StringWriter();
    }
}

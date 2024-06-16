namespace TucaAPI.src.Providers
{
    public interface ITemplateRenderingProvider
    {
        StringWriter Render(string relativePath, object data);
    }
}


namespace TucaAPI.src.Interfaces
{
    public interface ITemplateRenderingService
    {
        StringWriter Render(string relativePath, object data);
    }
}
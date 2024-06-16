using HandlebarsDotNet;
using TucaAPI.src.Providers;

namespace TucaAPI.src.Provider
{
    public class TemplateRenderingProvider : ITemplateRenderingProvider
    {
        public StringWriter Render(string relativePath, object data)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

            using var fileStream = File.OpenRead(filePath);
            using var source = new StreamReader(fileStream);

            var handlebars = Handlebars.Create();
            var template = handlebars.Compile(source);

            using var writer = new StringWriter();
            template(writer, data);

            return writer;
        }
    }
}
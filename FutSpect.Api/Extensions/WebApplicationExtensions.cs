using Microsoft.Extensions.FileProviders;

namespace FutSpect.Api.Extensions;

public static class WebApplicationExtensions
{
    public static void ServeFavicon(this WebApplication app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(app.Environment.ContentRootPath),
            OnPrepareResponse = ctx =>
            {
                if (!ctx.File.Name.Equals("favicon.ico", StringComparison.OrdinalIgnoreCase))
                {
                    ctx.Context.Response.StatusCode = 404;
                    ctx.Context.Response.Body = Stream.Null;
                }
            }
        });
    }
}
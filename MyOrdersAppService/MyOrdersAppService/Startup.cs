using Microsoft.Owin;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Owin;

namespace MyOrdersAppService
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new FileServerOptions()
            {
                RequestPath = new PathString(""),
                FileSystem = new PhysicalFileSystem("client"),
                EnableDefaultFiles = true
            };

            options.DefaultFilesOptions.DefaultFileNames.Add("index.html");
            options.StaticFileOptions.ServeUnknownFileTypes = true;

            app.UseFileServer(options);
        }
    }
}
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;

namespace OrdersService
{
    public  class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            var httpConfig = new HttpConfiguration();

            httpConfig.MapHttpAttributeRoutes();
            
            httpConfig.Formatters.Clear();
            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = {ContractResolver = new CamelCasePropertyNamesContractResolver()}
            };
            httpConfig.Formatters.Add(formatter);

            httpConfig.EnableSwagger(c=> c.SingleApiVersion("v1", "Order Service API LIVE"))
                .EnableSwaggerUi();

            httpConfig.EnableCors(
                new EnableCorsAttribute("*", "*", "*"));

            app.UseWebApi(httpConfig);
        }
    }
}
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.Cors;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin.Cors;
using Newtonsoft.Json.Serialization;
using OrdersService.Properties;
using Owin;
using Swashbuckle.Application;

namespace OrdersService
{
    public  class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            JwtSecurityTokenHandler.InboundClaimTypeMap = new Dictionary<string, string>();
            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = Settings.Default.IdSrvBaseUrl,
                RequiredScopes = new List<string> { "ordersapi"}
            });

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

            httpConfig.Filters.Add(new AuthorizeAttribute());

            app.UseWebApi(httpConfig);
        }
    }
}
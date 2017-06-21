using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace OrdersService.Controllers
{
    [RoutePrefix("api/health")]
    // NOTE: maybe put default logic into OWIN middleware
    public class HealthController : ApiController
    {
        [HttpGet]
        [Route("ping")]
        [AllowAnonymous]
        public string Ping()
        {
            return "OK";
        }
    }
}

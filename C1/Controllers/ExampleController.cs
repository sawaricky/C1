using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace C1.Controllers
{
    public class ExampleController : ApiController
    {
        [HttpPost]
        [Route("api/Example/Test")]
        public string Test()
        {
            return "hello";
        }
    }
}

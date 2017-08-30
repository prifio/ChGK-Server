using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Hanabi;

namespace Hanabi_ASP.Controllers
{
    public class LogInController : ApiController
    {
        [HttpPost]
        public bool Post([FromBody] LogInInfo s)
        {
            return true;
        }
    }

    public class LogInInfo
    {
        public string Nick { get; set; }
        public string Password { get; set; }
    }
}

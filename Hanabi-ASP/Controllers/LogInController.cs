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
        public Dictionary<int, string> Post([FromBody] LogInInfo s)
        {
            var ans = new Dictionary<int, string>();
            ans.Add(10, "20");
            ans.Add(30, "id");
            return ans;
        }
    }

    public class LogInInfo
    {
        public string Nick { get; set; }
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Indigox.UUM.Application.Web
{
    public class WindowsAuthenticationHandler : IHttpHandler
    {
        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Redirect("/uum/index.htm");
        }
    }
}

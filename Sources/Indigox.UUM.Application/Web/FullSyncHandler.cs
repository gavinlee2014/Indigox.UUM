using Indigox.UUM.Application.Sync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace Indigox.UUM.Application.Web
{
    public class FullSyncHandler : IHttpHandler
    {
        public bool IsReusable => false;

        public void ProcessRequest(HttpContext context)
        {

            int sysID = Convert.ToInt32(context.Request["sysID"]);

            if (sysID == 0)
            {
                return;
            }
            FullSyncCommand command = new FullSyncCommand()
            {
                SystemID = sysID
            };
            command.Execute();
        }

    }
}

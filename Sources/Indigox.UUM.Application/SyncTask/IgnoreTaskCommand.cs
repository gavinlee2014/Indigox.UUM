using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.Sync;
using Indigox.UUM.Sync.Interfaces;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.SyncTask
{
    public class IgnoreTaskCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public int ID { get; set; }

        public void Execute()
        {
            var task = SyncManager.GetTaskByID(ID);

            if (task != null)
            {
                task.SetIgnore();
            }
            else
            {
                Log.Debug(string.Format("Can't find task [{0}].", ID));
            }
        }
    }
}

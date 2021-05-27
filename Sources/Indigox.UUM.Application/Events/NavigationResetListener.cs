using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.EventBus.Interface.Event;
using Indigox.Common.StateContainer;
using Indigox.Settings.Interfaces.Navi;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.Events
{
    public class NavigationResetListener
    {
        public void ResetNavigation(object source, IEvent evt)
        {
            Log.Error("Reset Navigation");
            Log.Debug("Reset Navigation");
            StateContext.Current.Application["Navigation"] = new Dictionary<string, IList<INavigation>>();
        }
    }
}

using System;
using System.Collections.Generic;
using Indigox.UUM.Sync.Model;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Utils
{
    internal class RegisteredSysConfiguration
    {
        private const string TAG = "OpusOnePowerHRP";

        public static SysConfiguration Get()
        {
            IList<SysConfiguration> sources = SysConfigurationService.Instance.GetSysConfigurations( SyncType.Source );

            foreach ( var source in sources )
            {
                if ( source.ClientName == TAG )
                {
                    return source;
                }
            }

            return null;
        }
    }
}
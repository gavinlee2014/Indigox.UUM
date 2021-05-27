using System;
using Indigox.Common.Logging;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    internal class DummySynchronizer : ISynchronizer
    {
        private string entityType;
        private string changeType;

        public DummySynchronizer( string entityType, string changeType )
        {
            this.entityType = entityType;
            this.changeType = changeType;
        }

        public void Synchronize( string id )
        {
            Log.Debug( string.Format( "dummy:{{entityType={0},changeType={1},id={2}}}", entityType, changeType, id ) );
        }
    }
}
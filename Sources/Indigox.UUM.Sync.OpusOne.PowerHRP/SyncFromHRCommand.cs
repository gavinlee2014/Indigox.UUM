using System;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP
{
    public class SyncFromHRCommand : Indigox.Web.CQRS.Interface.ICommand
    {
        public void Execute()
        {
            Synchronizer synchronizer = new Synchronizer( Databases.OpusOnePowerHRP, Databases.UUM );
            synchronizer.SynchronizeChanges();
            synchronizer.SynchronizeData();
        }
    }
}
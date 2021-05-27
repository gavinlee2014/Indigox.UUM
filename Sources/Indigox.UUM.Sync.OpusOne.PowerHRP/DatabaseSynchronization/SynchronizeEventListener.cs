using System;
using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization
{
    public interface SynchronizeEventListener
    {
        void RecordInserted( Table table, string keyValue, IRecord newRecord );

        void RecordRemoved( Table table, string keyValue, IRecord oldRecord );

        void RecordUpdated( Table table, string keyValue, FieldCollection changedFields, IRecord newRecord, IRecord oldRecord );
    }
}
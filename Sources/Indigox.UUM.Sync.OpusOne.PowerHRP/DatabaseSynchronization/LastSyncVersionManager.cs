using System;
using Indigox.Common.Data.Interface;
using Indigox.Common.Logging;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization
{
    internal class LastSyncVersionManager
    {
        private IDatabase backupDatabase;

        public LastSyncVersionManager( IDatabase backupDatabase )
        {
            this.backupDatabase = backupDatabase;
        }

        public void SetLastSyncVersion( string entityType, long lastSyncVersion )
        {
            Log.Debug( string.Format( "set {0} lastSyncVersion to {1}", entityType, lastSyncVersion ) );

            backupDatabase.ExecuteText(
                SQL_SAVE,
                "@EntityType varchar(255), @LastSyncVersion bigint",
                entityType,
                lastSyncVersion );
        }

        public bool TryGetLastSyncVersion( string entityType, ref long lastSyncVersion )
        {
            object value = backupDatabase.ScalarText(
                SQL_LOAD,
                "@EntityType varchar(255)",
                entityType );

            if ( value == null )
            {
                return false;
            }
            else
            {
                lastSyncVersion = (long)value;
                Log.Debug( string.Format( "get {0} lastSyncVersion: {1}", entityType, lastSyncVersion ) );
                return true;
            }
        }

        #region SQLS

        private static readonly string SQL_LOAD = @"select LastSyncVersion from OpusOneHRVersion where EntityType=@EntityType;";

        private static readonly string SQL_SAVE = @"if exists (select 1 from OpusOneHRVersion where EntityType=@EntityType)
update OpusOneHRVersion set LastSyncVersion=@LastSyncVersion where EntityType=@EntityType;
else
insert into OpusOneHRVersion (LastSyncVersion, EntityType) values (@LastSyncVersion, @EntityType);";

        #endregion SQLS
    }
}
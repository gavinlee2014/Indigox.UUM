using System;
using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization
{
    internal class ChangeRecordManager
    {
        internal const string CT_DELETE = "D";
        internal const string CT_INSERT = "I";
        internal const string CT_UPDATE = "U";

        private IDatabase backupDatabase;
        private IDatabase sourceDatabase;

        private LastSyncVersionManager lastSyncVersionManager;

        public ChangeRecordManager( IDatabase sourceDatabase, IDatabase backupDatabase )
        {
            this.sourceDatabase = sourceDatabase;
            this.backupDatabase = backupDatabase;

            lastSyncVersionManager = new LastSyncVersionManager( backupDatabase );
        }

        public IRecordSet GetChangeRecords()
        {
            return backupDatabase.QueryText( SQL_QUERY_NOT_PROCESSED_CHANGES );
        }

        public void SyncAllRecords()
        {
            Table[] tables = TableManager.Instance.GetTables();
            foreach ( Table table in tables )
            {
                SyncRecords( table );
            }
        }

        public void UpdateChangeRecordProcessed( long id )
        {
            backupDatabase.ExecuteText( SQL_UPDATE_CHANGE_RECORD_PROCESSED, "@id bigint", id );
        }

        private void Insert( string entityType, string entityID, string changeType, long changeVersion )
        {
            ICommand cmd = backupDatabase.CreateTextCommand( SQL_INSERT_CHANGE_RECORD );
            cmd.AddParameter( "@EntityType varchar(255)", entityType );
            cmd.AddParameter( "@EntityID varchar(500)", entityID );
            cmd.AddParameter( "@ChangeType char(1)", changeType );
            cmd.AddParameter( "@ChangeVersion bigint", changeVersion );
            backupDatabase.Execute( cmd );
        }

        private void SyncRecords( Table table )
        {
            string tableName = table.Name;
            string keyName = table.Key.Fields[ 0 ].Name;
            long lastSyncVersion = 0;
            IRecordSet rs;
            ICommand cmd;

            if ( lastSyncVersionManager.TryGetLastSyncVersion( tableName, ref lastSyncVersion ) )
            {
                cmd = sourceDatabase.CreateTextCommand( SQL_QUERY_CHANGED_ENTITY );
            }
            else
            {
                cmd = sourceDatabase.CreateTextCommand( SQL_QUERY_ALL_ENTITY_AS_INSERTED );
            }
            cmd.AddParameter( "$table", tableName );
            cmd.AddParameter( "$pk", keyName );
            cmd.AddParameter( "@LastSyncVersion bigint output", lastSyncVersion );
            rs = sourceDatabase.Query( cmd );

            lastSyncVersion = (long)cmd.GetParameter( "@LastSyncVersion" ).Value;

            foreach ( IRecord record in rs.Records )
            {
                string changeType = record.GetString( "ChangeType" );
                long changeVersion = record.GetLong( "ChangeVersion" );
                string keyValue = Convert.ToString( record.GetValue( keyName ) );
                Insert( tableName, keyValue, changeType, changeVersion );
            }

            lastSyncVersionManager.SetLastSyncVersion( tableName, lastSyncVersion );
        }

        #region SQLS

        private static readonly string SQL_INSERT_CHANGE_RECORD = @"
insert into OpusOneHRChanges (EntityType, EntityID, ChangeType, ChangeVersion, Processed)
values (@EntityType, @EntityID, @ChangeType, @ChangeVersion, 0)
";

        private static readonly string SQL_QUERY_ALL_ENTITY_AS_INSERTED = @"
select 'I' AS [ChangeType],
       a.sys_change_version [ChangeVersion],
       t.$pk
  from $table t
  left join changetable(changes $table, null) a on t.$pk = a.$pk;

set @LastSyncVersion = CHANGE_TRACKING_CURRENT_VERSION();
";

        private static readonly string SQL_QUERY_CHANGED_ENTITY = @"
select a.sys_change_operation as [ChangeType],
       a.sys_change_version [ChangeVersion],
       a.$pk
  from changetable(changes $table, @LastSyncVersion) a;

set @LastSyncVersion = CHANGE_TRACKING_CURRENT_VERSION();
";

        private static readonly string SQL_QUERY_NOT_PROCESSED_CHANGES = @"
select ID,
       EntityType,
       EntityID,
       ChangeType,
       Processed
  from OpusOneHRChanges
 where Processed = 0
 order by ID";

        private static readonly string SQL_UPDATE_CHANGE_RECORD_PROCESSED = @"update OpusOneHRChanges set Processed = 1 where ID = @id";

        #endregion SQLS
    }
}
using System;
using System.Collections.Generic;
using Indigox.Common.Data.Interface;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization
{
    public class Synchronizer
    {
        private IDatabase backupDatabase;
        private IDatabase sourceDatabase;
        private ChangeRecordManager changeRecordManager;

        public Synchronizer( IDatabase sourceDatabase, IDatabase backupDatabase )
        {
            this.sourceDatabase = sourceDatabase;
            this.backupDatabase = backupDatabase;

            this.changeRecordManager = new ChangeRecordManager( sourceDatabase, backupDatabase );
        }

        public void SynchronizeChanges()
        {
            this.changeRecordManager.SyncAllRecords();
        }

        public void SynchronizeData()
        {
            IRecordSet rs = changeRecordManager.GetChangeRecords();
            foreach ( IRecord record in rs.Records )
            {
                Dispatch( record );
            }
        }

        private bool Dispatch( IRecord change )
        {
            long id = change.GetLong( "ID" );
            string entityType = change.GetString( "EntityType" );
            string entityID = change.GetString( "EntityID" );
            string changeType = change.GetString( "ChangeType" );

            try
            {
                switch ( changeType )
                {
                    case ChangeRecordManager.CT_INSERT:
                        DispatchInsert( entityType, entityID );
                        break;

                    case ChangeRecordManager.CT_UPDATE:
                        DispatchUpdate( entityType, entityID );
                        break;

                    case ChangeRecordManager.CT_DELETE:
                        DispatchDelete( entityType, entityID );
                        break;
                }

                changeRecordManager.UpdateChangeRecordProcessed( id );

                return true;
            }
            catch ( Exception ex )
            {
                ApplicationException warppedEx = new ApplicationException( string.Format( "Synchronize {0} failed.", id ), ex );
                Log.Error( warppedEx.ToString() );

                //throw warppedEx;
                return false;
            }
        }

        private void DispatchDelete( string entityType, string entityID )
        {
            Table table = TableManager.Instance.GetTable( entityType );
            IRecordSet backupRecordSet = backupDatabase.QueryText( table.GetSelectBackupRecordSql( entityID ) );

            if ( backupRecordSet.Records.Count == 0 )
            {
                //?
                return;
            }

            IRecord backupRecord = backupRecordSet.Records[ 0 ];

            Log.Debug( string.Format( "DispatchDelete {0}: {1}", entityType, entityID ) );

            if ( table.EventListener != null )
            {
                table.EventListener.RecordRemoved( table, entityID, backupRecord );
            }

            DeleteFromBackupTable( table, backupRecord );
        }

        private void DeleteFromBackupTable( Table table, IRecord backupRecord )
        {
            string sql = table.GetDeleteBackupRecordSql( backupRecord );
            backupDatabase.ExecuteText( sql );
        }

        private void DispatchInsert( string entityType, string entityID )
        {
            Table table = TableManager.Instance.GetTable( entityType );
            IRecordSet sourceRecordSet = sourceDatabase.QueryText( table.GetSelectSourceRecordSql( entityID ) );

            if ( sourceRecordSet.Records.Count == 0 )
            {
                //?
                return;
            }

            IRecord sourceRecord = sourceRecordSet.Records[ 0 ];

            Log.Debug( string.Format( "DispatchInsert {0}: {1}", entityType, entityID ) );

            if ( table.EventListener != null )
            {
                table.EventListener.RecordInserted( table, entityID, sourceRecord );
            }

            InsertIntoBackupTable( table, sourceRecord );
        }

        private void InsertIntoBackupTable( Table table, IRecord sourceRecord )
        {
            string sql = table.GetInsertBackupRecordSql( sourceRecord );
            backupDatabase.ExecuteText( sql );
        }

        private void DispatchUpdate( string entityType, string entityID )
        {
            Table table = TableManager.Instance.GetTable( entityType );
            IRecordSet sourceRecordSet = sourceDatabase.QueryText( table.GetSelectSourceRecordSql( entityID ) );
            IRecordSet backupRecordSet = backupDatabase.QueryText( table.GetSelectBackupRecordSql( entityID ) );

            if ( sourceRecordSet.Records.Count == 0 || backupRecordSet.Records.Count == 0 )
            {
                //?
                return;
            }

            IRecord sourceRecord = sourceRecordSet.Records[ 0 ];
            IRecord backupRecord = backupRecordSet.Records[ 0 ];

            List<Field> changedFields = new List<Field>();
            foreach ( Field field in table.Fields )
            {
                string sourceValue=Convert.ToString(sourceRecord.GetValue(field.Name));
                string backupValue=Convert.ToString(backupRecord.GetValue(field.Name));
                if (sourceValue!=backupValue)
                {
                    changedFields.Add(field);
                }
            }

            if ( changedFields.Count == 0 )
            {
                // no changes
                return;
            }

            Log.Debug( string.Format( "DispatchUpdate {0}: {1}", entityType, entityID ) );

            if ( table.EventListener != null )
            {
                table.EventListener.RecordUpdated( table, entityID, new FieldCollection( changedFields ), sourceRecord, backupRecord );
            }

            UpdateBackupRecord( table, sourceRecord, changedFields );
        }

        private void UpdateBackupRecord( Table table, IRecord sourceRecord, List<Field> changedFields )
        {
            string sql = table.GetUpdateBackupRecordSql( sourceRecord, changedFields );
            backupDatabase.ExecuteText( sql );
        }
    }
}
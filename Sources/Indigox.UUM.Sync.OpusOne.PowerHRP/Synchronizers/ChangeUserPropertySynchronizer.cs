using System;
using System.Collections.Generic;
using Indigox.Common.Data.Interface;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    internal class ChangeUserPropertySynchronizer : ISynchronizer
    {
        private SysConfiguration source = RegisteredSysConfiguration.Get();
        private static Dictionary<string, string> NameMapping;

        public ChangeUserPropertySynchronizer()
        {
            if ( NameMapping == null )
            {
                NameMapping = new Dictionary<string, string>();
                NameMapping.Add( "c_name", "Name" );
                NameMapping.Add( "homephone", "Tel" );
                NameMapping.Add( "mobile", "Mobile" );
                NameMapping.Add( "servicestatus", "Enabled" );
            }
        }

        public void Synchronize( string id )
        {
            IRecordSet hrRecordSet = Databases.OpusOnePowerHRP.QueryText(
                 "select serialnumber,servicestatus,c_name,email,mobile,homephone,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5 from employee1  where serialnumber ='" + id + "'" );
            IRecordSet backupRecordSet = Databases.UUM.QueryText(
               "select serialnumber,servicestatus,c_name,email,mobile,homephone,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5 from OpusOneEmployee  where serialnumber ='" + id + "'" );
            if ( hrRecordSet.Records.Count == 0 || backupRecordSet.Records.Count == 0 )
            {
                return;
            }
            IRecord hrRecord = hrRecordSet.Records[ 0 ];
            IRecord backupRecord = backupRecordSet.Records[ 0 ];
            IDictionary<string, object> propertyChanges = new Dictionary<string, object>();
            string hrParentID = GetParentID( hrRecord );
            string backupParentID = GetParentID( backupRecord );
            if ( hrParentID != backupParentID )
            {
                propertyChanges.Add( "ParentID", hrParentID );
            }
            foreach ( IColumn col in hrRecord.Columns )
            {
                if ( !NameMapping.ContainsKey( col.Name ) ) continue;
                string colName = col.Name;
                object hrValue = hrRecord.GetValue( colName );
                if ( hrValue.ToString() != backupRecord.GetString( colName ) )
                {
                    propertyChanges.Add( NameMapping[ colName ], hrValue );
                }
            }
            CheckPropertyChange( hrRecord, propertyChanges );
            CheckDisabled( hrRecord, backupRecord );
            CheckEnabled( hrRecord, backupRecord );
            UpdateBackup( hrRecord, propertyChanges );
        }

        private string GetParentID( IRecord record )
        {
            return string.Format( "{0}{1}{2}{3}{4}",
                        record.GetString( "orgcode1" ), record.GetString( "orgcode2" ), record.GetString( "orgcode3" ),
                        record.GetString( "orgcode4" ), record.GetString( "orgcode5" ) );
        }

        private void UpdateBackup( IRecord hrRecord, IDictionary<string, object> propertyChanges )
        {
            if ( propertyChanges.Count == 0 ) return;
            string sql = "Update OpusOneEmployee set  ";
            foreach ( string k in propertyChanges.Keys )
            {
                sql += "   " + k + "='" + propertyChanges[ k ] + "',";
            }
            sql = sql.Substring( 0, sql.Length - 1 );
            Databases.UUM.ExecuteText( sql );
        }

        private void CheckPropertyChange( IRecord hrRecord, IDictionary<string, object> propertyChanges )
        {
            ISyncTaskBuilder taskBuilder = new ChangeUserTaskBuilder()
            {
                Source = source,
                UserID = hrRecord.GetString( "serialnumber" ),
                Name = hrRecord.GetString( "c_name" ),
                PropertyChanges = propertyChanges
            };

            ISyncTask task = taskBuilder.Build();

            RepositoryFactory.Instance.CreateRepository<ISyncTask>().Add( task );
            SyncManager.AddTask( source.ClientName, task );
        }

        private void CheckDisabled( IRecord hrRecord, IRecord backupRecord )
        {
            if ( hrRecord.GetString( "servicestatus" ).ToLower() == "false" && backupRecord.GetString( "servicestatus" ).ToLower() == "true" )
            {
                ISyncTaskBuilder taskBuilder = new DisableUserTaskBuilder()
                {
                    Source = source,
                    UserID = hrRecord.GetString( "serialnumber" ),
                    Name = hrRecord.GetString( "name" )
                };

                ISyncTask task = taskBuilder.Build();

                RepositoryFactory.Instance.CreateRepository<ISyncTask>().Add( task );
                SyncManager.AddTask( source.ClientName, task );
            }
        }

        private void CheckEnabled( IRecord hrRecord, IRecord backupRecord )
        {
            if ( hrRecord.GetString( "servicestatus" ).ToLower() == "true" && backupRecord.GetString( "servicestatus" ).ToLower() == "false" )
            {
                ISyncTaskBuilder taskBuilder = new EnableUserTaskBuilder()
                {
                    Source = source,
                    UserID = hrRecord.GetString( "serialnumber" ),
                    Name = hrRecord.GetString( "name" )
                };

                ISyncTask task = taskBuilder.Build();

                RepositoryFactory.Instance.CreateRepository<ISyncTask>().Add( task );
                SyncManager.AddTask( source.ClientName, task );
            }
        }
    }
}
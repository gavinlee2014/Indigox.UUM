using System.Collections.Generic;
using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;
using Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Setting;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    public class EmployeeSynchronizer : SynchronizeEventListener
    {
        private SysConfiguration source = RegisteredSysConfiguration.Get();

        public void RecordInserted( Table table, string keyValue, IRecord newRecord )
        {
            string name = newRecord.GetString( "c_name" );

            if ( string.IsNullOrEmpty( name ) )
            {
                return;
            }

            SynchronizeCreateUser( newRecord );
        }

        public void RecordRemoved( Table table, string keyValue, IRecord oldRecord )
        {
            string serialnumber = oldRecord.GetString( "serialnumber" );
            string name = oldRecord.GetString( "c_name" );

            SynchronizeDeleteUser( serialnumber, name );
        }

        public void RecordUpdated( Table table, string keyValue, FieldCollection changedFields, IRecord newRecord, IRecord oldRecord )
        {
            string serialnumber = newRecord.GetString( "serialnumber" );
            string name = newRecord.GetString( "c_name" );
            string oldname = oldRecord.GetString( "c_name" );

            if ( string.IsNullOrEmpty( name ) )
            {
                return;
            }

            if ( string.IsNullOrEmpty( oldname ) )
            {
                RecordInserted( table, keyValue, newRecord );
                return;
            }

            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();

            if ( changedFields.Contains( "c_name" ) )
            {
                propertyChanges.Add( "Name", newRecord.GetString( "c_name" ) );
            }
            if (changedFields.Contains("businessphone"))
            {
                propertyChanges.Add("Tel", newRecord.GetString("businessphone"));
            }
            if (changedFields.Contains("employeeflag"))
            {
                propertyChanges.Add("EmployeeFlag", newRecord.GetValue("employeeflag"));
            }
            if (changedFields.Contains("portrait"))
            {
                propertyChanges.Add("Portrait", newRecord.GetValue("portrait"));
            }
            if ( changedFields.Contains( "mobile" ) )
            {
                propertyChanges.Add( "Mobile", newRecord.GetString( "mobile" ) );
            }
            if ( changedFields.Contains( "email" ) )
            {
                propertyChanges.Add( "Email", newRecord.GetString( "email" ) );
            }
            /*
             * 修改日期：2018-09-21
             * 修改人：曾勇
             * 修改內容：HR系统的Employee表变化时候，将离职日期添加到propertyChanges中，
             *         原有逻辑是在禁用时候再从OpusOneEmployee表中读取离职日期QuitDate，
             *         但是有可能因为去读取时，OpusOneEmployee还未Commit，导致未获取到离职日期
             */
            if (changedFields.Contains("quitdate"))
            {
                Log.Error("OpusOne Employee " + serialnumber + " change quitdate!" + newRecord.GetDateTime("quitdate").ToShortDateString());
                propertyChanges.Add("Quitdate", newRecord.GetDateTime("quitdate"));
            }
            if ( changedFields.ContainsAny( new string[] { "orgcode1", "orgcode2", "orgcode3", "orgcode4", "orgcode5" } ) )
            {
                string parentID = GetOrganizationID( newRecord );
                propertyChanges.Add( "ParentID", parentID );
            }

            if ( propertyChanges.Count > 0 )
            {
                if (changedFields.Contains("servicestatus"))
                {
                    SynchronizePropertyChange(serialnumber, name, propertyChanges);
                }
                else
                {
                    string servicestatus = newRecord.GetString("servicestatus").ToLower();
                    if (servicestatus == "true" || servicestatus == "1")
                    {
                        SynchronizePropertyChange(serialnumber, name, propertyChanges);
                    }
                }
            }

            if ( changedFields.Contains( "servicestatus" ) )
            {
                string servicestatus = newRecord.GetString( "servicestatus" ).ToLower();

                if ( servicestatus == "false" || servicestatus == "0" )
                {
                    SynchronizeDisableUser( serialnumber, name );
                }
                else
                {
                    SynchronizeEnableUser( serialnumber, name );
                }
            }
        }

        private string GetOrganizationID( IRecord record )
        {
            string id = "";
            HRIgnoreOrg ignoreOrg = new HRIgnoreOrg();
            for (int i = 1; i <= 5; i++)
            {
                string orgcode = record.GetString("orgcode" + i);
                if (orgcode == ignoreOrg.IgnoreOrg) continue;
                id += orgcode;
            }
            return id;
        }

        private void SynchronizeCreateUser( IRecord record )
        {
            // email is generated in UUM
            //email = record.GetString( "email" ),

            ISyncTaskBuilder taskBuilder = new CreateUserTaskBuilder()
            {
                Source = source,
                NativeID = record.GetString( "serialnumber" ),
                OrganizationalUnitID = GetOrganizationID( record ),
                Name = record.GetString( "c_name" ).Trim(),
                Mobile = record.GetString( "mobile" ),
                Telephone = record.GetString("businessphone"),
                OtherContact=record.GetValue("employeeflag").ToString(),
                Portrait=record.GetValue("portrait").ToString()
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask( source.ClientName, task );
        }

        private void SynchronizeDeleteUser( string serialnumber, string name )
        {
            ISyncTaskBuilder taskBuilder = new DeleteUserTaskBuilder()
            {
                Source = source,
                UserID = serialnumber,
                Name = name
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask( source.ClientName, task );
        }

        private void SynchronizeDisableUser( string serialnumber, string name )
        {
            ISyncTaskBuilder taskBuilder = new DisableUserTaskBuilder()
            {
                Source = source,
                UserID = serialnumber,
                Name = name
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask( source.ClientName, task );
        }

        private void SynchronizeEnableUser( string serialnumber, string name )
        {
            ISyncTaskBuilder taskBuilder = new EnableUserTaskBuilder()
            {
                Source = source,
                UserID = serialnumber,
                Name = name
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask( source.ClientName, task );
        }

        private void SynchronizePropertyChange( string serialnumber, string name, IDictionary<string, object> propertyChanges )
        {
            ISyncTaskBuilder taskBuilder = new ChangeUserTaskBuilder()
            {
                Source = source,
                UserID = serialnumber,
                Name = name,
                PropertyChanges = propertyChanges
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask( source.ClientName, task );
        }
    }
}
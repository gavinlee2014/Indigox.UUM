using System;
using System.Collections.Generic;
using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;
using Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;
using Indigox.Common.Logging;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    public class EmployeePositionSynchronizer : SynchronizeEventListener
    {
        private IDatabase sourceDatabase = Databases.OpusOnePowerHRP;
        private IDatabase backupDatabase = Databases.UUM;

        private SysConfiguration source = RegisteredSysConfiguration.Get();

        private void UpdateOpusOneEmployeeTitle(string serialnumber, string title)
        {
            string sql = "update OpusOneEmployee set title=@title where serialnumber=@serialnumber";
            backupDatabase.ExecuteText(sql, "@title varchar(100), @serialnumber nvarchar(10)", title, serialnumber);
        }

        private string GetOpusOneEmployeeTitle(string serialnumber)
        {
            string sql = "select title from OpusOneEmployee where  serialnumber=@serialnumber";
            IRecordSet recordSet = backupDatabase.QueryText(sql,"@serialnumber nvarchar(10)", serialnumber);
            if (recordSet != null && recordSet.Records.Count == 1)
            {
                return recordSet.Records[0].GetString("title");
            }
            return null;
        }

        public void RecordInserted( Table table, string keyValue, IRecord newRecord )
        {
            string positioncode = newRecord.GetString( "positioncode" );
            string serialnumber = newRecord.GetString( "serialnumber" );
            /*
             * 修改時間：2018-10-08
             * 修改人：曾勇
             * 修改内容：因为HR系统中使用 currentposition和defaultposition这2个字段来标识当前有效的职务，所以调整逻辑
             */
            string currentPosition = Convert.ToString( newRecord.GetValue("currentposition"));
            bool defaultPosition = Convert.ToBoolean(newRecord.GetValue("defaultposition")) ;
            Log.Debug("EmployeePosition Synchronizer currentPosition is :" + currentPosition + " && defaultposition is :" + defaultPosition);
            if ( !(currentPosition.Equals("1") && defaultPosition == true))
            {
                Log.Debug("Not Needed currentPosition is :" + currentPosition + " && defaultposition is :" + defaultPosition);
                return;
            }

            string name = null;
            string title = null;

            string sql1 = "select c_name from OpusOnePosition t1 where t1.positioncode=@positioncode";
            IRecordSet rs1 = backupDatabase.QueryText( sql1, "@positioncode varchar", positioncode );
            if ( rs1.Records.Count == 1 )
            {
                title = rs1.Records[ 0 ].GetString( "c_name" );
            }
            else
            {
                return;
            }

            string oldTitle = GetOpusOneEmployeeTitle(serialnumber);
            if (title == oldTitle) return;

            UpdateOpusOneEmployeeTitle(serialnumber, title);

            string sql2 = "select c_name from OpusOneEmployee t1 where t1.servicestatus = 1 and t1.serialnumber=@serialnumber";
            IRecordSet rs2 = backupDatabase.QueryText( sql2, "@serialnumber varchar", serialnumber );
            if ( rs2.Records.Count == 1 )
            {
                name = rs2.Records[ 0 ].GetString( "c_name" );
            }
            else
            {
                return;
            }

            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();
            propertyChanges.Add( "Title", title );

            SynchronizePropertyChange( serialnumber, name, propertyChanges );
        }

        public void RecordRemoved( Table table, string keyValue, IRecord oldRecord )
        {
            string positioncode = oldRecord.GetString( "positioncode" );
            string serialnumber = oldRecord.GetString( "serialnumber" );
            string currentPosition = Convert.ToString(oldRecord.GetValue("currentposition"));
            string defaultPosition = Convert.ToString(oldRecord.GetValue("defaultposition"));
            //bool defaultPosition = Convert.ToBoolean(oldRecord.GetValue("defaultposition"));
            Log.Error("EmployeePosition Synchronizer currentPosition is :" + currentPosition + " && defaultposition is :" + defaultPosition);
            if (!(currentPosition.Equals("1") && defaultPosition.Equals("1")))
            {
                Log.Error("Not Needed to Update Title { serialnumber :" + serialnumber + "}" );
                return;
            }

            string name = null;
            string title = null;

            string sql2 = "select c_name from OpusOneEmployee t1 where t1.serialnumber=@serialnumber";
            IRecordSet rs2 = backupDatabase.QueryText( sql2, "@serialnumber varchar", serialnumber );
            if ( rs2.Records.Count == 1 )
            {
                name = rs2.Records[ 0 ].GetString( "c_name" );
            }
            else
            {
                return;
            }

            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();
            propertyChanges.Add( "Title", title );

            SynchronizePropertyChange( serialnumber, name, propertyChanges );
        }

        public void RecordUpdated( Table table, string keyValue, FieldCollection changedFields, IRecord newRecord, IRecord oldRecord )
        {
            // do the same as Insert
            RecordInserted( table, keyValue, newRecord );
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
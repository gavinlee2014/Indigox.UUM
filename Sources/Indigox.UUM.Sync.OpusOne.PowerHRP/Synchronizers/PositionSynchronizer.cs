using System;
using System.Collections.Generic;
using Indigox.Common.Data.Interface;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;
using Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    public class PositionSynchronizer : SynchronizeEventListener
    {
        private IDatabase sourceDatabase = Databases.OpusOnePowerHRP;
        private IDatabase backupDatabase = Databases.UUM;

        private SysConfiguration source = RegisteredSysConfiguration.Get();

        public void RecordInserted( Table table, string keyValue, IRecord newRecord )
        {
            Log.Debug( string.Format( "Ignore RecordInserted: {0}", keyValue ) );
        }

        public void RecordRemoved( Table table, string keyValue, IRecord oldRecord )
        {
            Log.Debug( string.Format( "Ignore RecordRemoved: {0}", keyValue ) );
        }

        public void RecordUpdated( Table table, string keyValue, FieldCollection changedFields, IRecord newRecord, IRecord oldRecord )
        {
            string positioncode = newRecord.GetString( "positioncode" );
            string title = newRecord.GetString( "c_name" );

            /*
            string sql = @"select t2.serialnumber, t2.c_name 
from OpusOneEmployeePosition t1 
join OpusOneEmployee t2 on t1.serialnumber=t2.serialnumber 
where t1.positioncode=@positioncode";
            string sql = @"SELECT t2.serialnumber, t2.c_name
FROM (
	SELECT p.serialnumber, MAX(p.autoid) AS pid
	FROM OpusOneEmployeePosition p
	GROUP BY p.serialnumber
) tmp, OpusOneEmployeePosition t1, OpusOneEmployee t2
WHERE tmp.pid = t1.autoid
	AND t1.serialnumber = t2.serialnumber
	AND t2.servicestatus = 1
	AND t1.positioncode = @positioncode";
            */
            /*
             * 修改时间：2018-08-23
             * 修改人：曾勇
             * 修改原因：HR系统里人员角色记录有多条记录，以最新的一条为准，同时只更新HR系统里未离职的用户
             * 修改时间：2018-10-08
             * 修改人：曾勇
             * 修改原因：逻辑调整，HR系统表OpusOneEmployeePosition中
             * currentposition = 1 and defaultposition = 1 表示为有效角色
             */
            string sql = @"SELECT t2.serialnumber, t2.c_name
FROM OpusOneEmployeePosition t1, OpusOneEmployee t2
WHERE t1.serialnumber = t2.serialnumber
	AND t1.currentposition = 1
	AND t1.defaultposition = 1
	AND t2.servicestatus = 1
	AND t1.positioncode = @positioncode";            

            IRecordSet rs = backupDatabase.QueryText(sql, "@positioncode varchar", positioncode);

            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();
            propertyChanges.Add( "Title", title );

            foreach ( IRecord record in rs.Records )
            {
                string serialnumber = record.GetString( "serialnumber" );
                string name = record.GetString( "c_name" );
                SynchronizePropertyChange( serialnumber, name, propertyChanges );
            }
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
using System;
using System.Collections.Generic;
using System.Text;
using Indigox.Common.Data.Interface;
using Indigox.Common.Logging;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization;
using Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Setting;
using Indigox.UUM.Sync.OpusOne.PowerHRP.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    public class OrganizationSynchronizer : SynchronizeEventListener
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
            int typeid = newRecord.GetInt( "typeid" );
            string code = newRecord.GetString( "code" );

            IList<IRecord> structures = GetOrganizationalStrucure( code, typeid );

            foreach ( IRecord orgStructureRecord in structures )
            {
                UpdateOrganizationStructure( orgStructureRecord );
            }
        }

        /// <summary>
        /// 获取受影响的OrganizationalStrucure
        /// 1. Organization对应的OrganizationStructures
        /// 2. 用到Organization的Name的Organization，所对应的OrganizationStructures
        /// </summary>
        /// <param name="orgCode"></param>
        /// <param name="typeid"></param>
        /// <returns></returns>
        private IList<IRecord> GetOrganizationalStrucure( string orgCode, int typeid )
        {
            string ignoreOrg = ( new HRIgnoreOrg() ).IgnoreOrg;

            //string sql = string.Format( "select " + GetOpusOneOrganizationalStrucureFields() + " from opusoneorganizationstructure where ( typeid=4 and orgcode2={0} and orgcode3={1} ) or (orgcode{2}={3} and typeid={4} and orgcode{5}<>{6})",
            //    ignoreOrg, orgCode, typeid, orgCode, typeid, typeid, ignoreOrg );

            string sql = "select $fields from opusoneorganizationstructure"
                       + " where ( typeid = 4 and orgcode2 = @ignoreOrg and orgcode3 = @orgCode )"
                       + " or ( orgcode$typeid = @orgCode and typeid = @typeid and orgcode$typeid <> @ignoreOrg )";

            ICommand command = backupDatabase.CreateTextCommand( sql )
                .AddParameter( "@ignoreOrg", ignoreOrg )
                .AddParameter( "@orgCode", orgCode )
                .AddParameter( "@typeid", typeid )
                .AddParameter( "$typeid", typeid.ToString() )
                .AddParameter( "$fields", GetOpusOneOrganizationalStrucureFields() );

            IRecordSet rs = backupDatabase.Query( command );

            return rs.Records;
        }

        private string GetOpusOneOrganizationalStrucureFields()
        {
            Table table = TableManager.Instance.GetTable( "organizationstructure" );
            IList<Field> fields = table.Fields;
            StringBuilder builder = new StringBuilder();
            foreach ( Field field in fields )
            {
                builder.Append( field.Name );
                builder.Append( "," );
            }
            if ( fields.Count > 0 )
            {
                builder.Remove( builder.Length - 1, 1 );
            }
            return builder.ToString();
        }

        private void UpdateOrganizationStructure( IRecord orgStructureRecord )
        {
            var orgStructureSynchronizer = new OrganizationStructureSynchronizer();
            int typeid = orgStructureRecord.GetInt( "typeid" );
            string organizationID = orgStructureSynchronizer.GetOrganizationID( orgStructureRecord, typeid );
            string name = orgStructureSynchronizer.GetOrganizationName( orgStructureRecord );

            Dictionary<string, object> propertyChanges = new Dictionary<string, object>();

            propertyChanges.Add( "Name", name );

            SynchronizePropertyChange( organizationID, name, propertyChanges );
        }

        private void SynchronizePropertyChange( string organizationID, string name, IDictionary<string, object> propertyChanges )
        {
            ChangeOrganizationTaskBuilder taskBuilder = new ChangeOrganizationTaskBuilder()
            {
                Name = name,
                OrganizationalUnitID = organizationID,
                PropertyChanges = propertyChanges,
                Source = source
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask( source.ClientName, task );
        }
    }
}
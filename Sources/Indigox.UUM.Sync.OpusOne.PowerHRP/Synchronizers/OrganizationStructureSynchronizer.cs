using System;
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
    public class OrganizationStructureSynchronizer : SynchronizeEventListener
    {
        private IDatabase sourceDatabase = Databases.OpusOnePowerHRP;

        private SysConfiguration source = RegisteredSysConfiguration.Get();

        public void RecordInserted(Table table, string keyValue, IRecord newRecord)
        {
            int typeid = newRecord.GetInt("typeid");
            if (typeid == 0)
            {
                return;
            }

            SynchronizeOrganizationCreated(newRecord);
        }

        public void RecordRemoved(Table table, string keyValue, IRecord oldRecord)
        {
            int typeid = oldRecord.GetInt("typeid");
            if (typeid == 0)
            {
                return;
            }

            SynchronizeOrganizationDeleted(oldRecord);
        }

        public void RecordUpdated(Table table, string keyValue, FieldCollection changedFields, IRecord newRecord, IRecord oldRecord)
        {
            int typeid = newRecord.GetInt("typeid");
            if (typeid == 0)
            {
                return;
            }

            Log.Debug(string.Format("Ignore RecordUpdated: {0}", keyValue));
        }

        internal string GetOrganizationID(IRecord record, int typeid)
        {
            string id = "";
            HRIgnoreOrg ignoreOrg = new HRIgnoreOrg();
            for (int i = 1; i <= typeid; i++)
            {
                string orgcode = record.GetString("orgcode" + i);
                if (orgcode == ignoreOrg.IgnoreOrg) continue;
                id += orgcode;
            }
            return id;
        }

        internal string GetOrganizationName(IRecord record)
        {
            string organizationName = "";
            int typeid = record.GetInt("typeid");
            string orgCode = record.GetString("orgcode" + typeid);
            string parentCode = string.Empty, parentName = string.Empty;
            HRIgnoreOrg ignoreOrg = new HRIgnoreOrg();
            string type2Code = record.GetString("orgcode2");
            if (typeid == 4 && type2Code == ignoreOrg.IgnoreOrg)
            {
                parentCode = record.GetString("orgcode3");
            }
            if (parentCode != string.Empty)
            {
                IRecordSet rs = sourceDatabase.QueryText("select c_name from organization where code = '" + parentCode + "'");
                if (rs.Records.Count > 0)
                {
                    parentName = rs.Records[0].GetString("c_name").TrimEnd();
                }
            }

            IRecordSet recordSet = sourceDatabase.QueryText("select c_name from organization where code = '" + orgCode + "'");
            if (recordSet.Records.Count > 0)
            {
                organizationName = recordSet.Records[0].GetString("c_name").TrimEnd(); ;
            }

            return parentName + organizationName;
        }

        private string GetOrganizationType(IRecord record)
        {
            int typeid = record.GetInt("typeid");
            if (typeid > 1)
            {
                HRIgnoreOrg ignoreOrg = new HRIgnoreOrg();
                string orgCode2 = record.GetString("orgcode2");
                if (orgCode2 == ignoreOrg.IgnoreOrg)
                {
                    typeid--;
                }
            }

            string organizationType = "";
            switch (typeid)
            {
                case 1:
                    organizationType = "Corporation";
                    break;
                case 2:
                case 3:
                    organizationType = "Company";
                    break;
                case 4:
                    organizationType = "Department";
                    break;
                case 5:
                    organizationType = "Section";
                    break;
            }
            return organizationType;
        }

        private void SynchronizeOrganizationCreated(IRecord record)
        {
            ISyncTaskBuilder taskBuilder = new CreateOrganizationTaskBuilder()
            {
                Source = source,
                NativeID = GetOrganizationID(record, record.GetInt("typeid")),
                ParentOrganizationalUnitID = GetOrganizationID(record, record.GetInt("typeid") - 1),
                Name = GetOrganizationName(record),
                OrderNum = record.GetInt("structureorder"),
                OrganizationalUnitType = GetOrganizationType(record)
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask(source.ClientName, task);
        }

        private void SynchronizeOrganizationDeleted(IRecord record)
        {
            ISyncTaskBuilder taskBuilder = new DeleteOrganizationTaskBuilder()
            {
                Source = source,
                OrganizationalUnitID = GetOrganizationID(record, record.GetInt("typeid")),
                Name = GetOrganizationName(record)
            };

            ISyncTask task = taskBuilder.Build();
            SyncManager.AddTask(source.ClientName, task);
        }
    }
}
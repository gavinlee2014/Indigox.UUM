using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Setting;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    public class CreateOrganizationStructureSynconizer : ISynchronizer
    {
        public void Synchronize(string id)
        {
            IRecordSet rs = Databases.OpusOnePowerHRP.QueryText(
                "select structurecode,structureorder,typeid,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5 from organizationstructure " +
            " where typeid<>0 and structurecode ='" + id + "'");

            if (rs.Records.Count > 0)
            {
                IRecord record = rs.Records[0];
                string code = record.GetString("orgcode" + record.GetInt("typeid"));//organization code
                HRIgnoreOrg ignoreOrg = new HRIgnoreOrg();
                if (code == ignoreOrg.IgnoreOrg) return;

                this.BuildTask(record);
                this.Backup(record);
            }
        }

        private void BuildTask(IRecord record)
        {
            SysConfiguration source = RegisteredSysConfiguration.Get();

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
            RepositoryFactory.Instance.CreateRepository<ISyncTask>().Add(task);
            SyncManager.AddTask(source.ClientName, task);
        }
        private string GetOrganizationID(IRecord record, int typeid)
        {
            string id = "";
            HRIgnoreOrg ignoreOrg=new HRIgnoreOrg ();
            for (int i = 1; i <= typeid; i++)
            {
                string orgcode=record.GetString("orgcode" + i);
                if(orgcode==ignoreOrg.IgnoreOrg) continue;
                id += orgcode;
            }
            return id;
        }
        private string GetOrganizationName(IRecord record)
        {
            string organizationName = "";
            int typeid = record.GetInt("typeid");
            string orgCode = record.GetString("orgcode" + typeid);
            string parentCode=string.Empty,parentName=string.Empty;
            HRIgnoreOrg ignoreOrg = new HRIgnoreOrg();
            string type2Code=record.GetString("orgcode2");
            if (typeid == 4 && type2Code==ignoreOrg.IgnoreOrg)
            {
                parentCode = record.GetString("orgcode3");
            }
            if (parentCode != string.Empty)
            {
                IRecordSet rs= Databases.OpusOnePowerHRP.QueryText("select c_name from organization where code = '" + parentCode + "'");
                if (rs.Records.Count > 0)
                {
                    parentName = rs.Records[0].GetString("c_name").TrimEnd();
                }
            }

            IRecordSet recordSet = Databases.OpusOnePowerHRP.QueryText("select c_name from organization where code = '" + orgCode + "'");
            if (recordSet.Records.Count > 0)
            {
                organizationName = recordSet.Records[0].GetString("c_name");
            }

            return parentName + organizationName;
        }

        private string GetOrganizationType(IRecord record)
        {
            string organizationType = "Department";
            return organizationType;
        }


        private void Backup(IRecord record)
        {
            string sql = string.Format(@"
            insert into OpusOneOrganizationStructure 
            (structurecode,structureorder,typeid,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5) 
            values
            ({0},{1},{2},'{3}','{4}','{5}','{6}','{7}')",
            record.GetInt("structurecode"),
            record.GetInt("structureorder"),
            record.GetInt("typeid"),
            record.GetString("orgcode1"),
            record.GetString("orgcode2"),
            record.GetString("orgcode3"),
            record.GetString("orgcode4"),
            record.GetString("orgcode5"));
            Databases.UUM.ExecuteText(sql);
        }
    }
}

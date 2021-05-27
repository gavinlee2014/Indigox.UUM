using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;
using Indigox.Common.DomainModels.Factory;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Setting;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    internal class CreateUserSynchronizer : ISynchronizer
    {
        public void Synchronize(string id)
        {          
            IRecordSet rs = Databases.OpusOnePowerHRP.QueryText(
                "select serialnumber,empcode,c_name,homephone,mobile,email,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5,servicestatus from employee1 " +
            " where c_name<>'' and serialnumber ='" + id + "'");

            if (rs.Records.Count > 0)
            {
                IRecord record = rs.Records[0];
                this.BuildTask(record);
                this.Backup(record);
            }
        }

        private void BuildTask(IRecord record)
        {
            SysConfiguration source = RegisteredSysConfiguration.Get();

            ISyncTaskBuilder taskBuilder = new CreateUserTaskBuilder()
            {
                Source = source,
                nativeID=record.GetString("serialnumber"),
                organizationalUnitID = GetOrganizationID(record),
                name = record.GetString("c_name"),
                email = record.GetString("email"),
                mobile = record.GetString("mobile"),
                telephone = record.GetString("homephone")               
            };

            ISyncTask task= taskBuilder.Build();

            RepositoryFactory.Instance.CreateRepository<ISyncTask>().Add(task);
            SyncManager.AddTask(source.ClientName, task);

        }
        private string GetOrganizationID(IRecord record)
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
        private void Backup(IRecord record)
        {
            string sql = string.Format(@"
            insert into opusoneemployee 
            (serialnumber,empcode,c_name,homephone,mobile,email,orgcode1,orgcode2,orgcode3,orgcode4,orgcode5,servicestatus) 
            values
            ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
            record.GetString("serialnumber"),
            record.GetString("empcode"),
            record.GetString("c_name"),
            record.GetString("homephone"),
            record.GetString("mobile"),
            record.GetString("email"),
            record.GetString("orgcode1"),
            record.GetString("orgcode2"),
            record.GetString("orgcode3"),
            record.GetString("orgcode4"),
            record.GetString("orgcode5"),
            record.GetString("servicestatus"));
            Databases.UUM.ExecuteText(sql);
        }
    }
}

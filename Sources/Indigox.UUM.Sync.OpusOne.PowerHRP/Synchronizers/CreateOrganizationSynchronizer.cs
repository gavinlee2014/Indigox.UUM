using System;
using System.Collections.Generic;
using Indigox.Common.Data.Interface;
using Indigox.UUM.Sync.Interfaces;
using Indigox.UUM.Sync.Model;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers.TaskBuilders;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Utils;
using Indigox.UUM.Sync.OpusOne.PowerHRP.Setting;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Synchronizers
{
    internal class CreateOrganizationSynchronizer : ISynchronizer
    {
        public void Synchronize(string id)
        {
            HRIgnoreOrg ignoreOrg = new HRIgnoreOrg();

            IRecordSet rs = Databases.OpusOnePowerHRP.QueryText(
                "select code,typeid,c_name from organization where code<>'"+ignoreOrg.IgnoreOrg+"' and code='"+id+"'");

            if (rs.Records.Count > 0)
            {
                IRecord record = rs.Records[0];
                this.Backup(record);
            }
        }

        private void Backup(IRecord record)
        {
            string sql = string.Format(@"
            insert into opusOneOrganization
            (code,typeid,c_name) 
            values
            ('{0}',{1},'{2}')",
            record.GetString("code"),
            record.GetInt("typeid"),
            record.GetString("c_name"));
           
            Databases.UUM.ExecuteText(sql);
        }
    }
}
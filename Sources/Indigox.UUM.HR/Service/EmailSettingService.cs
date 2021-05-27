using Indigox.Common.DomainModels.Factory;
using Indigox.Common.Membership.Interfaces;
using Indigox.UUM.HR.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.HR.Service
{
    public class EmailSettingService
    {
        private IDictionary<string, string> suffixMapping;
        private string defaultSuffix;

        public static EmailSettingService Instance
        {
            get
            {
                return Nested.instance;
            }
        }
        private EmailSettingService()
        {
            suffixMapping = new Dictionary<string, string>();
            HREmployeeEmailSetting setting = new HREmployeeEmailSetting();
            if (!String.IsNullOrEmpty(setting.EmailSuffix))
            {
                string[] list = setting.EmailSuffix.Split(';');
                foreach (var item in list)
                {
                    string[] pair = item.Split('=');
                    suffixMapping.Add(pair[0], pair[1]);
                }
            }
            defaultSuffix = setting.DefaultEmailSuffix;
        }

        public string GetSuffixByHRID(string parentHRObjectID)
        {
            if (String.IsNullOrEmpty(parentHRObjectID))
            {
                return defaultSuffix;
            }
            string parentOrgID = MappingUtil.GetPrincipalIDByHRObjectID(parentHRObjectID);
            return GetSuffix(parentOrgID);
        }

        public string GetSuffix(string parentOrgID)
        {
            if (suffixMapping.Keys.Count == 0)
            {
                return defaultSuffix;
            }
            if (String.IsNullOrEmpty(parentOrgID))
            {
                return defaultSuffix;
            }
            if (!suffixMapping.ContainsKey(parentOrgID))
            {
                IOrganizationalUnit parentOrg = RepositoryFactory.Instance.CreateRepository<IOrganizationalUnit>().Get(parentOrgID);
                if (parentOrg == null)
                {
                    return defaultSuffix;
                }
                return GetSuffix(parentOrg.Organization.ID);
            }
            else
            {
                return suffixMapping[parentOrgID];
            }
        }

        private class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested()
            {
            }

            internal static readonly EmailSettingService instance = new EmailSettingService();
        }
    }
}

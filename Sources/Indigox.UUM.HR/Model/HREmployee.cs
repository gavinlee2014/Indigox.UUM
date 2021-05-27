using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.HR.Model
{
    public enum HRState
    {
        Created, Changed, Deleted, Enabled, Disabled, Undefined
    }
    public class HREmployee
    {
        public string ID { get; set; }
        public string ParentID { get; set; }
        public string ParentName { get; set; }
        public string Name { get; set; }
        public string IdCard { get; set; }
        public string AccountName { get; set; }
        public string DisplayName { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public bool Enabled { get; set; }
        public bool EmployeeFlag { get; set; }
        public HRState State { get; set; }
        public bool Synchronized { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Portrait { get; set; }
        public string Description { get; set; }
        public bool HasPolyphone { get; set; }
        public string MailDatabase { get; set; }
        public double OrderNum { get; set; }
        public IDictionary<string, string> ExtendProperties { get; set; } = new Dictionary<string, string>();
        public IList<HROrganizationalRole> OrganizationalRoles { get; set; }

        private DateTime quitDate = DateTime.Parse("2099-01-01");

        public DateTime QuitDate
        {
            get { return quitDate; }
            set { quitDate = value; }
        }
        public IDictionary<string, string> CloneExtendProperties()
        {
            Dictionary<string, string> ret = new Dictionary<string, string>(this.ExtendProperties.Count);
            foreach (KeyValuePair<string, string> entry in this.ExtendProperties)
            {
                if (entry.Value != null)
                {
                    ret.Add(entry.Key, (string)entry.Value.Clone());
                }
                else
                {
                    ret.Add(entry.Key, "");
                }
            }
            return ret;
        }
    }
}

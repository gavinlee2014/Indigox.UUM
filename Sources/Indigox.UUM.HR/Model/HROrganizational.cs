using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Indigox.UUM.HR.Model
{
    public class HROrganizational
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ParentID { get; set; }
        public string ParentName { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }
        public HRState State { get; set; }
        public bool Synchronized { get; set; }
        public DateTime ModifyTime { get; set; }
        public string Description { get; set; }
        public IDictionary<string, string> ExtendProperties { get; set; } = new Dictionary<string, string>();

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

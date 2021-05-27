using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Factory
{
    public abstract class PrincipalFactory
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public double OrderNum { get; set; }
        public string DisplayName { get; set; }
        public IDictionary<string, string> ExtendProperties { get; set; } = new Dictionary<string, string>();

        protected void SetBaseProperties(IMutablePrincipal principal)
        {
            principal.Name = this.Name;
            principal.FullName = this.FullName;
            principal.Email = this.Email;
            principal.Description = this.Description;
            principal.OrderNum = this.OrderNum == 0 ? 1.001 : this.OrderNum;
            principal.DisplayName = this.DisplayName;
            principal.ExtendProperties = this.ExtendProperties;
        }

        public abstract string GetNextID();
    }
}

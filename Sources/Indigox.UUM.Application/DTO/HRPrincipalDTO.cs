using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.UUM.HR.Model;

namespace Indigox.UUM.Application.DTO
{
    public enum HRPrincipalType
    {
        HREmployee,
        HROgnazizational
    }
    public class HRPrincipalDTO
    {
        private HRState state;
        private HRPrincipalType principalType;

        public string ID { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string ParentID { get; set; }
        public int State
        {
            get
            {
                return (int)state;
            }
            set
            {
                this.state = (HRState)value;
            }
        }
        public bool Synchronized { get; set; }
        public int PrincipalType
        {
            get
            {
                return (int)this.principalType;
            }
            set
            {
                this.principalType = (HRPrincipalType)value;
            }
        }
        public DateTime ModifyTime { get; set; }
    }
}

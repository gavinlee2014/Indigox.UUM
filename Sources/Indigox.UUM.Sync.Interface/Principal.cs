using System;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface
{
    [Serializable()]
    [XmlType( Namespace = Consts.Namespace )]
    [XmlInclude( typeof( Group ) )]
    [XmlInclude( typeof( OrganizationalObject ) )]
    [XmlInclude( typeof( Role ) )]
    public abstract class Principal
    {
        private string fullName;
        private string id;
        private string name;
        private string email;
        private string description;
        private string displayName;


        private double orderNum;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public double OrderNum
        {
            get { return orderNum; }
            set { orderNum = value; }
        }
    }
}
using System;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface
{
    [Serializable()]
    [XmlType( Namespace = Consts.Namespace )]
    [XmlInclude( typeof( OrganizationalRole ) )]
    [XmlInclude( typeof( OrganizationalUnit ) )]
    [XmlInclude( typeof( OrganizationalPerson ) )]
    public abstract class OrganizationalObject : Principal
    {
        private string organizationID;

        public string OrganizationID
        {
            get { return organizationID; }
            set { organizationID = value; }
        }
    }
}
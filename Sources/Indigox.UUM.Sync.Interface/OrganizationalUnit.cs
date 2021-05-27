using System;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface
{
    [Serializable()]
    [XmlType( Namespace = Consts.Namespace )]
    public class OrganizationalUnit : OrganizationalObject
    {
        private string organizationalType;

        public string OrganizationalType
        {
            get { return organizationalType; }
            set { organizationalType = value; }
        }
    }
}
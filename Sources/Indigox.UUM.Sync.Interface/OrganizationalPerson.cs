using System;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface
{
    [Serializable()]
    [XmlType( Namespace = Consts.Namespace )]
    public class OrganizationalPerson : OrganizationalObject
    {
        private string account;
        private string title;
        private string mobile;
        private string telephone;
        private string fax;
        private string profile;
        private string idcard;
        private bool enabled = true;

        public string Account
        {
            get { return account; }
            set { account = value; }
        }

        public string IdCard
        {
            get { return idcard; }
            set { idcard = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
    }
}
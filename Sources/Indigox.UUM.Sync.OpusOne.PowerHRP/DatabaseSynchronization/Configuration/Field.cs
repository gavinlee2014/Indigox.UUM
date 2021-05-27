using System;
using System.Xml;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    public class Field : IXmlConfigNode
    {
        private bool isKey;
        private string name;

        public Field()
            : this( false )
        {
        }

        public Field( bool isKey )
        {
            this.isKey = isKey;
        }

        public bool IsKey
        {
            get { return isKey; }
        }

        public string Name
        {
            get { return name; }
        }

        public void Read( XmlElement element )
        {
            name = element.GetAttribute( "name" );
        }
    }
}
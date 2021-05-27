using System;
using System.Collections.Generic;
using System.Xml;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    public class FieldGroup : IXmlConfigNode
    {
        private List<Field> fields = new List<Field>();

        public List<Field> Fields
        {
            get { return fields; }
        }

        public void Read( XmlElement element )
        {
            foreach ( XmlElement fieldNode in element.SelectNodes( "field" ) )
            {
                Field field = new Field();
                field.Read( fieldNode );
                fields.Add( field );
            }
        }
    }
}
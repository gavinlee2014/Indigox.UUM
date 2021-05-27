using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    //public delegate void RecordSynchronizeEventHandler( EventArgs args );

    public class Table : IXmlConfigNode
    {
        //public RecordSynchronizeEventHandler RecordInserted;
        //public RecordSynchronizeEventHandler RecordRemoved;
        //public RecordSynchronizeEventHandler RecordUpdated;

        private string backupTable;
        private SynchronizeEventListener eventListener;
        private List<Field> fields = new List<Field>();
        private Key key = new Key();
        private string name;

        public string BackupTable
        {
            get { return backupTable; }
            set { backupTable = value; }
        }

        public SynchronizeEventListener EventListener
        {
            get { return eventListener; }
        }

        public List<Field> Fields
        {
            get { return fields; }
        }

        public Key Key
        {
            get { return key; }
        }

        public string Name
        {
            get { return name; }
        }

        public void Read( XmlElement element )
        {
            name = element.GetAttribute( "name" );
            backupTable = element.GetAttribute( "backupTable" );

            foreach ( XmlElement keyNode in element.SelectNodes( "key" ) )
            {
                key.Read( keyNode );
                fields.AddRange( key.Fields );
            }

            foreach ( XmlElement fieldNode in element.SelectNodes( "field" ) )
            {
                Field field = new Field();
                field.Read( fieldNode );
                fields.Add( field );
            }

            foreach ( XmlElement eventListenerNode in element.SelectNodes( "eventListener" ) )
            {
                Type type = Type.GetType( eventListenerNode.GetAttribute( "type" ), true );
                eventListener = (SynchronizeEventListener)Activator.CreateInstance( type );
            }

            //foreach ( XmlElement insertEventHandlerNode in element.SelectNodes( "insert/eventHandler" ) )
            //{
            //    MethodInfo method = ReadMethod( insertEventHandlerNode );
            //    RecordInserted += (RecordSynchronizeEventHandler)Delegate.CreateDelegate( typeof( RecordSynchronizeEventHandler ), method );
            //}

            //foreach ( XmlElement updateEventHandlerNode in element.SelectNodes( "update/eventHandler" ) )
            //{
            //    MethodInfo method = ReadMethod( updateEventHandlerNode );
            //    RecordUpdated += (RecordSynchronizeEventHandler)Delegate.CreateDelegate( typeof( RecordSynchronizeEventHandler ), method );
            //}

            //foreach ( XmlElement deleteEventHandlerNode in element.SelectNodes( "delete/eventHandler" ) )
            //{
            //    MethodInfo method = ReadMethod( deleteEventHandlerNode );
            //    RecordRemoved += (RecordSynchronizeEventHandler)Delegate.CreateDelegate( typeof( RecordSynchronizeEventHandler ), method );
            //}
        }

        //private MethodInfo ReadMethod( XmlElement eventHandlerNode )
        //{
        //    string methodName = eventHandlerNode.GetAttribute( "method" );
        //    string typeName = eventHandlerNode.GetAttribute( "type" );
        //    string assemblyName = eventHandlerNode.GetAttribute( "assembly" );
        //
        //    Assembly assembly = Assembly.Load( assemblyName );
        //    Type type = assembly.GetType( typeName );
        //    MethodInfo method = type.GetMethod( methodName );
        //
        //    return method;
        //}
    }
}
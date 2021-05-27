using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using Indigox.Common.Logging;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    public class TableManager
    {
        private static TableManager instance = new TableManager();

        private Table[] tables = new Table[] { };

        private TableManager()
        {
            try
            {
                Configure();
            }
            catch ( Exception ex )
            {
                Log.Error( ex.ToString() );
            }
        }

        public static TableManager Instance
        {
            get { return instance; }
        }

        public Table GetTable( string name )
        {
            foreach ( Table table in tables )
            {
                if ( StringComparer.CurrentCultureIgnoreCase.Equals( table.Name, name ) )
                {
                    return table;
                }
            }
            return null;
        }

        public Table[] GetTables()
        {
            return tables;
        }

        private void Configure()
        {
            // Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.config.xml
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream stream = assembly.GetManifestResourceStream( "Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.config.xml" );
            TextReader reader = new StreamReader( stream );
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load( reader );

            XmlElement configNode = (XmlElement)xdoc.SelectSingleNode( "/config" );
            if ( configNode == null )
            {
                throw new ApplicationException( "No config element." );
            }

            string defaultBackupTablePrefix = configNode.GetAttribute( "defaultBackupTablePrefix" );

            List<Table> temp = new List<Table>();
            foreach ( XmlElement tableNode in configNode.SelectNodes( "table" ) )
            {
                Table table = new Table();
                table.Read( tableNode );
                if ( string.IsNullOrEmpty( table.BackupTable ) )
                {
                    table.BackupTable = defaultBackupTablePrefix + table.Name;
                }
                temp.Add( table );
            }

            tables = temp.ToArray();
        }
    }
}
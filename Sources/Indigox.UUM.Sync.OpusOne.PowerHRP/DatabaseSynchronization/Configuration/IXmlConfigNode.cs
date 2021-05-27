using System;
using System.Xml;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.DatabaseSynchronization.Configuration
{
    internal interface IXmlConfigNode
    {
        void Read( XmlElement element );
    }
}
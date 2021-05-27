using System;
using System.Xml.Serialization;

namespace Indigox.UUM.Sync.Interface
{
    [Serializable()]
    [XmlType( Namespace = Consts.Namespace )]
    public class Group : Principal
    {
    }
}
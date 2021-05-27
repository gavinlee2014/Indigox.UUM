using System;

namespace Indigox.UUM.Sync.Interface
{
    public class PropertyChange
    {
        public PropertyChange()
        {
        }

        public PropertyChange( string Name, object Value )
        {
            this.Name = Name;
            this.Value = Value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }
}
using System;

namespace Indigox.UUM.Sync.Interface
{
    public class HRPropertyChange
    {
        public HRPropertyChange()
        {
        }

        public HRPropertyChange(string Name, string Value)
        {
            this.Name = Name;
            this.Value = Value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}
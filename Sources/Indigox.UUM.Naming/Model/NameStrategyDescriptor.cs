using System;
using Indigox.Common.Utilities;

namespace Indigox.UUM.Naming.Model
{
    public class NameStrategyDescriptor
    {
        public int ID { get; set; }
        public float Priority { get; set; }
        public bool Enabled { get; set; }
        public string Assembly { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public DateTime LastModifyTime { get; set; }

        public INameStrategy ConvertToNamestrategy()
        {
            return (INameStrategy)ReflectUtil.CreateInstance( this.ClassName + "," + this.Assembly );
        }
    }
}
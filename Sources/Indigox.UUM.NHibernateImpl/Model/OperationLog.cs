using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.StateContainer;

namespace Indigox.UUM.NHibernateImpl.Model
{
    public class OperationLog
    {
        public int ID { get; set; }
        public string Operator { get; set; }
        public string Operation { get; set; }
        public DateTime OperationTime { get; set; }
        public string DetailInformation { get; set; }
    }
}

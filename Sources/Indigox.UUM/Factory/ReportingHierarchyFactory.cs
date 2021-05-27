using System;
using System.Collections.Generic;
using System.Text;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.Membership;
using Indigox.UUM.Util;

namespace Indigox.UUM.Factory
{
    class ReportingHierarchyFactory
    {
        //private static readonly ReportingHierarchyFactory instance = new ReportingHierarchyFactory();

        //public static ReportingHierarchyFactory Instance
        //{
        //    get { return instance; }
        //}

        //private ReportingHierarchyFactory()
        //{
        //}

        public string Name { get; set; }

        public IReportingHierarchy Create()
        {
            return this.Create(this.GetNextID());
        }

        public IReportingHierarchy Create(int id)
        {
            IReportingHierarchy item = new ReportingHierarchy();
            item.ID = id;
            item.Name = this.Name;
            return item;
        }

        public int GetNextID()
        {
            return Convert.ToInt32(UUMIdGernerator.GetNext());
        }
    }
}

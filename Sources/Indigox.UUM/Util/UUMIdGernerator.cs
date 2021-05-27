using System;
using System.Collections.Generic;
using System.Text;
using Indigox.Common.DomainModels.Factory;

namespace Indigox.UUM.Util
{
    internal class UUMIdGernerator
    {
        public static string GetNext()
        {
            string id = IdGeneratorFactory.GetIdGenerator().GetNextID<string>("UUM");
            while (id.Length < 10)
            {
                id = "0" + id;
            }
            return id;  
        }
    }
}

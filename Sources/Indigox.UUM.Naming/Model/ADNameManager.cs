using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;

namespace Indigox.UUM.Naming.Model
{
    public class ADNameManager : INameManager
    {
        public bool Contains( string name )
        {
            Forest forest = Forest.GetCurrentForest();
            GlobalCatalog gc = forest.FindGlobalCatalog();

            using (DirectorySearcher searcher = gc.GetDirectorySearcher())
            {
                searcher.Filter = String.Format("(sAMAccountName={0})", name);
                SearchResultCollection results = searcher.FindAll();
                return results.Count > 0;
            }
        }
    }
}

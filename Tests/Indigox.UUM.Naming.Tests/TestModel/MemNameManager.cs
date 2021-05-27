using System;
using System.Collections.Generic;
using Indigox.UUM.Naming.Model;

namespace Indigox.UUM.Naming.Tests.TestModel
{
    public class MemNameManager : INameManager
    {
        private static List<string> names = new List<string>();

        public static void AddName( string name )
        {
            names.Add( name );
        }

        public static void Clear()
        {
            names.Clear();
        }

        public bool Contains( string name )
        {
            return names.Contains( name );
        }
    }
}
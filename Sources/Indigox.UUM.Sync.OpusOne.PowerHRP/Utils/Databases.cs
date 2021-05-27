using System;
using Indigox.Common.Data;
using Indigox.Common.Data.Interface;

namespace Indigox.UUM.Sync.OpusOne.PowerHRP.Utils
{
    internal class Databases
    {
        private static IDatabase m_OpusOnePowerHRP = new DatabaseFactory().CreateDatabase( "OpusOnePowerHRP" );
        
        public static IDatabase OpusOnePowerHRP
        {
            get { return Databases.m_OpusOnePowerHRP; }
        }

        private static IDatabase m_UUM = new DatabaseFactory().CreateDatabase( "UUM" );
        
        public static IDatabase UUM
        {
            get { return Databases.m_UUM; }
        }
    }
}
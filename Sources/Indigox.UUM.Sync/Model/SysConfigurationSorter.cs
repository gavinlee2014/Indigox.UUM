using System;
using System.Collections.Generic;

namespace Indigox.UUM.Sync.Model
{
    public class SysConfigurationSorter
    {
        private List<SysConfiguration> waits = new List<SysConfiguration>();

        public IList<SysConfiguration> Sort( IList<SysConfiguration> sysConfigurations )
        {
            List<SysConfiguration> list = new List<SysConfiguration>( sysConfigurations.Count );

            foreach ( var value in sysConfigurations )
            {
                AddTo( list, value );
            }

            return list;
        }

        private void AddTo( List<SysConfiguration> list, SysConfiguration value )
        {
            if ( list.Contains( value ) )
            {
                return;
            }
            else if ( waits.Contains( value ) )
            {
                throw new ApplicationException( "Recycle dependencies: " + value.ClientName );
            }
            else
            {
                waits.Add( value );
                foreach ( var dependency in value.Dependencies )
                {
                    AddTo( list, dependency );
                }
                list.Add( value );
                waits.Remove( value );
            }
        }

        //private class SysConfigurationComparer : IComparer<SysConfiguration>
        //{
        //    public int Compare( SysConfiguration a, SysConfiguration b )
        //    {
        //        if ( a.Dependencies.Contains( b ) )
        //        {
        //            return 1;
        //        }
        //        else if ( b.Dependencies.Contains( a ) )
        //        {
        //            return -1;
        //        }
        //        return 0;
        //    }
        //}
    }
}
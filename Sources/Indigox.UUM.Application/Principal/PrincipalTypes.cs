using System;
using System.Collections.Generic;

namespace Indigox.UUM.Application.Principal
{
    public sealed class PrincipalTypes
    {
        private static Dictionary<string, int[]> Discriminators;

        static PrincipalTypes()
        {
            Discriminators = new Dictionary<string, int[]>();
            Discriminators.Add( "Principal", new int[] { 0 } );
            Discriminators.Add( "OrganizationalUnit", new int[] { 100, 199 } );
            Discriminators.Add( "Corporation", new int[] { 101 } );
            Discriminators.Add( "Company", new int[] { 102 } );
            Discriminators.Add( "Department", new int[] { 103 } );
            Discriminators.Add( "Section", new int[] { 104 } );
            Discriminators.Add( "User", new int[] { 200, 299 } );
            Discriminators.Add( "OrganizationalPerson", new int[] { 201 } );
            Discriminators.Add( "Group", new int[] { 300, 399 } );
            Discriminators.Add( "OrganizationalRole", new int[] { 400, 499 } );
            Discriminators.Add( "Role", new int[] { 500, 599 } );
        }

        public static int[] GetDiscriminatorRange( string typeName )
        {
            if ( Discriminators.ContainsKey( typeName ) )
            {
                return Discriminators[ typeName ];
            }
            else
            {
                return null;
            }
        }
    }
}
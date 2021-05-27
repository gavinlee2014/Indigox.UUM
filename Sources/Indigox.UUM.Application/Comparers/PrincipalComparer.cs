using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.Comparers
{
    class PrincipalComparer : IEqualityComparer<IPrincipal>
    {
        public bool Equals(IPrincipal p1, IPrincipal p2)
        {
            return p1.ID == p2.ID;
        }

        public int GetHashCode(IPrincipal p)
        {
            return p.ID.GetHashCode();
        }
    }
}
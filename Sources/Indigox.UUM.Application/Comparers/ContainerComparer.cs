using System.Collections.Generic;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.Comparers
{
    public class ContainerComparer : IEqualityComparer<IContainer>
    {
        public bool Equals(IContainer p1, IContainer p2)
        {
            return p1.ID == p2.ID;
        }

        public int GetHashCode(IContainer p)
        {
            return p.ID.GetHashCode();
        }
    }
}

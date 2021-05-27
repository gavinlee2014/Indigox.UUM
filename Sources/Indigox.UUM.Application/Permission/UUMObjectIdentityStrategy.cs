using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.DomainModels.Identity;
using Indigox.Common.DomainModels.Interface.Identity;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Application.Permission
{
    public class UUMObjectIdentityStrategy : ObjectIdentityStrategy
    {
        public static IObjectIdentity CreateObjectIdentify(object identifer)
        {
            return new ObjectIdentity((typeof(IPrincipal)).FullName, identifer.ToString());
        }
    }
}

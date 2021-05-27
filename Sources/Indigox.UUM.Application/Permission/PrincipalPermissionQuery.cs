using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Indigox.Common.DomainModels.Interface.Identity;
using Indigox.CMS.Security.Service;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.StateContainer;
using Indigox.CMS.Security.Interfaces;

namespace Indigox.UUM.Application.Permission
{
    public class PrincipalPermissionQuery : Indigox.Web.CQRS.GenericSingleQuery<Hashtable>
    {
        public string PrincipalID { get; set; }

        private static readonly string DefaultPrincipalID = "OR1000000000";

        public override Hashtable Single()
        {
            string principalID = this.PrincipalID;
            if (String.IsNullOrEmpty(principalID))
            {
                principalID = DefaultPrincipalID;
            }
            IObjectIdentity id = UUMObjectIdentityStrategy.CreateObjectIdentify(principalID);
            PermissionService permissionService = new PermissionService();
            IUser currentUser = StateContext.Current.Session.User;
            IPermission effectivePermission = permissionService.GetEffectivePermission(id, currentUser);

            Hashtable htEffectivePermission = new Hashtable();
            htEffectivePermission.Add("EffectivePermission", effectivePermission.Mask);
            return htEffectivePermission;
        }
    }
}

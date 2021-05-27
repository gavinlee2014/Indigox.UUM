using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Indigox.Common.Membership.Interfaces;
using Indigox.Common.StateContainer;
using Indigox.Common.Logging;

namespace Indigox.UUM.Application.Permission
{
    public class HRRoleQuery : Indigox.Web.CQRS.GenericSingleQuery<bool>
    {

        public override bool Single()
        {
            IUser currentUser = StateContext.Current.Session.User;

            bool isHRRole = false;
            bool isUMAdmin = false;

            foreach (IContainer container in currentUser.MemberOf)
            {
                Log.Error("Container Email is :" + container.Email);
                if (container.Email == "uumadmingroup@chngalaxy.com")
                {
                    isUMAdmin = true;
                }

                if (container.Email == "hrgroup@chngalaxy.com")
                {
                    isHRRole = true;
                }   
            }

            return isHRRole && !isUMAdmin;
        }
    }
}

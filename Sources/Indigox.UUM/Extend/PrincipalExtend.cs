using System.Text;
using Indigox.Common.Membership.Interfaces;

namespace Indigox.UUM.Extend
{
    public static class PrincipalExtend
    {
        public static string GetDescription(this IPrincipal principal)
        {
            string description = "";
            if (principal is IOrganizationalPerson) description = GetUserDescription(principal as IOrganizationalPerson);
            else if (principal is IGroup) description = GetGroupDescripton(principal as IGroup);
            else if (principal is IRole) description = GetRoleDescription(principal as IRole);
            else if (principal is IOrganizationalUnit) description = GetOrganizationUnitDescription(principal as IOrganizationalUnit);
            else if (principal is IOrganizationalRole) description = GetOrganizationalRoleDescription(principal as IOrganizationalRole);
            else description = "该对象不支持GetDescription方法";
            return description;
        }

        private static string GetUserDescription(IOrganizationalPerson user)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("登录名：" + user.AccountName);
            builder.Append("，所属部门：" + user.Organization.DisplayName);
            if (!string.IsNullOrEmpty(user.DisplayName))
            {
                builder.Append("，显示名：" + user.DisplayName);
            }
            if (!string.IsNullOrEmpty(user.Title))
            {
                builder.Append("，职称：" + user.Title);
            }
            if (!string.IsNullOrEmpty(user.Mobile))
            {
                builder.Append("，手机号码：" + user.Mobile);
            }
            if (!string.IsNullOrEmpty(user.Telephone))
            {
                builder.Append("，电话号码：" + user.Telephone);
            }
            if (!string.IsNullOrEmpty(user.Fax))
            {
                builder.Append("，传真：" + user.Fax);
            }
            if (!string.IsNullOrEmpty(user.Email))
            {
                builder.Append("，邮箱：" + user.Email);
            }
            if (!string.IsNullOrEmpty(user.OtherContact))
            {
                builder.Append("，其他联系方式：" + user.OtherContact);
            }
            if (!string.IsNullOrEmpty(user.Profile))
            {
                builder.Append("，头像：" + user.Profile);
            }
            return builder.ToString();
        }

        private static string GetOrganizationUnitDescription(IOrganizationalUnit org)
        {
            string description = string.Format("部门名称：{0}，Email：{1}", org.Name, org.Email);
            if (!string.IsNullOrEmpty(org.DisplayName))
            {
                description += "，显示名：" + org.DisplayName;
            }
            return description;
        }

        private static string GetOrganizationalRoleDescription(IOrganizationalRole orgRole)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("组织角色名称：" + orgRole.Name);
            builder.Append("，显示名称：" + orgRole.DisplayName);
            if (!string.IsNullOrEmpty(orgRole.Email))
            {
                builder.Append("，邮箱：" + orgRole.Email);
            }
            if (orgRole.Role != null)
            {
                builder.Append("，所属角色：" + orgRole.Role.Name);
            }
            return builder.ToString();
        }

        private static string GetRoleDescription(IRole role)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("角色名称：" + role.Name);
            builder.Append("，邮箱：" + role.Email);
            if (!string.IsNullOrEmpty(role.DisplayName))
            {
                builder.Append("，显示名称：" + role.DisplayName);
            }
            return builder.ToString();
        }

        private static string GetGroupDescripton(IGroup group)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("群组名称：" + group.Name);
            if (!string.IsNullOrEmpty(group.DisplayName))
            {
                builder.Append("，显示名称:" + group.DisplayName);
            }
            if (!string.IsNullOrEmpty(group.Email))
            {
                builder.Append("，邮箱：" + group.Email);
            }
            return builder.ToString();

        }

    }
}

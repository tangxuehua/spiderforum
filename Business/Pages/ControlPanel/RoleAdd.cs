using System;
using System.Web.UI;
using System.Web.Core;

namespace Forum.Business
{
    public class RoleAddPage : ForumBasePage
    {
        protected ValuedTextBox subjectTextBox;

        [AjaxMethod(IncludeControlValuesWithCallBack = true)]
        public void Save()
        {
            TRequest<Role> request = new TRequest<Role>();
            request.Data.Name.Value = subjectTextBox.Value;
            if (Engine.GetAll(request).Count > 0)
            {
                throw new Exception("½ÇÉ«Ãû³ÆÖØ¸´£¡");
            }

            Role role = new Role();
            role.Name.Value = subjectTextBox.Value;
            role.RoleType.Value = GetRoleDefaultType();
            Engine.Create(role);
        }

        private long GetRoleDefaultType()
        {
            long roleType = 0;
            roleType += Convert.ToInt64(((long)RoleType.AllowVisible).ToString(), 16);
            roleType += Convert.ToInt64(((long)RoleType.AllowDelete).ToString(), 16);
            roleType += Convert.ToInt64(((long)RoleType.AllowEditPermission).ToString(), 16);
            return roleType;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Web.Core;

namespace Forum.Business
{
    public class SectionRoleAndUser : Entity
    {
        private SectionRoleUser sectionRoleUser = new SectionRoleUser();
        private ForumUser user = new ForumUser();

        public SectionRoleUser SectionRoleUser
        {
            get
            {
                return sectionRoleUser;
            }
            set
            {
                sectionRoleUser = value;
            }
        }
        public ForumUser User
        {
            get
            {
                return user;
            }
            set
            {
                user = value;
            }
        }
    }
}
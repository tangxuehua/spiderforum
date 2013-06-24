using System;
using System.Collections.Generic;
using System.Web.Core;

namespace Forum.Business
{
    public class SectionRoleUser : Entity
    {
        private IntType sectionId = new IntType();
        private IntType roleId = new IntType();
        private IntType userId = new IntType();

        public IntType SectionId
        {
            get
            {
                return sectionId;
            }
            set
            {
                sectionId = value;
            }
        }
        public IntType RoleId
        {
            get
            {
                return roleId;
            }
            set
            {
                roleId = value;
            }
        }
        public IntType UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
            }
        }
    }
}
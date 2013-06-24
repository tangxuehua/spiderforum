namespace System.Web.Core
{
    public class UserRole : Entity
    {
        private IntType userId = new IntType();
        private IntType roleId = new IntType();

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
    }
}
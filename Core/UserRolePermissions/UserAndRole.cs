namespace System.Web.Core
{
    public class UserAndRole : Entity
    {
        private User user = new User();
        private Role role = new Role();

        public User User
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
        public Role Role
        {
            get
            {
                return role;
            }
            set
            {
                role = value;
            }
        }
    }
}
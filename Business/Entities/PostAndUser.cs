using System;
using System.Collections.Generic;
using System.Web.Core;

namespace Forum.Business
{
    public class PostAndUser : Entity
    {
        private Post post = new Post();
        private ForumUser user = new ForumUser();

        public Post Post
        {
            get
            {
                return post;
            }
            set
            {
                post = value;
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
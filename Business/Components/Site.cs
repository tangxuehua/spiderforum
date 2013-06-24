using System;
using System.Web.Core;
using System.Collections.Generic;

namespace Forum.Business
{
    public class ForumSite : BaseSite
    {
        protected override User CreateUserFromMemberInfo(MemberInfo memberInfo)
        {
            ForumUser user = new ForumUser();

            user.MemberId.Value = memberInfo.MemberId;
            user.NickName.Value = memberInfo.MemberAttributes["NickName"] as string;
            user.UserStatus.Value = (int)UserStatus.Normal;
            user.TotalMarks.Value = 500;

            return user;
        }
        protected override void DeleteUser(int userId)
        {
            //将该用户所发表的帖子的回复删除
            TRequest<Post> postRequest = new TRequest<Post>();
            postRequest.Data.AuthorId.Value = userId;

            //将该用户发表的帖子的发布状态设置为'已删除'
            //然后，论坛会通过Job的方式真正删除已被标记为'已删除'的帖子，TODO
            //并更新相应版块的统计信息（TotalPosts）
            ThreadRequest threadRequest = new ThreadRequest();
            threadRequest.Data.AuthorId.Value = userId;
            threadRequest.UpdatePropertyEntryList.Add(new UpdatePropertyEntry("ThreadReleaseStatus", (int)ThreadReleaseStatus.Deleted));

            //删除用户所拥有的角色
            TRequest<UserRole> userRoleRequest = new TRequest<UserRole>();
            userRoleRequest.Data.UserId.Value = userId;

            //删除用户在版块中的各种角色
            TRequest<SectionRoleUser> sectionRoleUserRequest = new TRequest<SectionRoleUser>();
            sectionRoleUserRequest.Data.UserId.Value = userId;

            Engine.Executes(
                BinderBuilder.BuildDeleteListBinder(postRequest),
                BinderBuilder.BuildUpdateListBinder(threadRequest),
                BinderBuilder.BuildDeleteListBinder(userRoleRequest),
                BinderBuilder.BuildDeleteListBinder(sectionRoleUserRequest),
                BinderBuilder.BuildDeleteBinder<ForumUser>(userId));
        }
    }
}
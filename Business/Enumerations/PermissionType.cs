namespace Forum.Business
{
    public enum PermissionType
    {
        Undefined = 0,                           //未定义的权限
        View = 0x0000000000000001,               //浏览帖子
        CreateThread = 0x0000000000000002,       //发表帖子
        EditThread = 0x0000000000000004,         //编辑帖子
        StickThread = 0x0000000000000008,        //置顶帖子
        ModifyThreadStatus = 0x0000000000000010, //修改帖子状态
        DeleteThread = 0x0000000000000020,       //删除帖子
        EditPost = 0x0000000000000040,           //编辑回复
        DeletePost = 0x0000000000000080,         //删除回复
        UserAdmin = 0x0000000000000100,          //用户管理
        RoleAdmin = 0x0000000000000200,          //角色管理
        RolePermissionAdmin = 0x0000000000000400,//角色授权管理
        SectionAdmin = 0x0000000000000800,       //版块及版块分组管理
        SectionAdminsAdmin = 0x0000000000001000, //版主管理
        CloseThread = 0x0000000000002000,        //结贴
        ExceptionLogAdmin = 0x0000000000004000,  //错误日志管理
    }
}
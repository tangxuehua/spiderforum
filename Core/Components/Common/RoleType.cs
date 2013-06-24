namespace System.Web.Core
{
    public enum RoleType
    {
        AllowVisible = 0x0000000000000001,               //在用户角色管理中允许显示
        AllowDelete = 0x0000000000000002,                //在角色管理中允许删除
        AllowEditPermission = 0x0000000000000004,        //在角色权限管理中允许修改
    }
}
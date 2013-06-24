namespace Forum.Business
{
    public enum PermissionType
    {
        Undefined = 0,                           //δ�����Ȩ��
        View = 0x0000000000000001,               //�������
        CreateThread = 0x0000000000000002,       //��������
        EditThread = 0x0000000000000004,         //�༭����
        StickThread = 0x0000000000000008,        //�ö�����
        ModifyThreadStatus = 0x0000000000000010, //�޸�����״̬
        DeleteThread = 0x0000000000000020,       //ɾ������
        EditPost = 0x0000000000000040,           //�༭�ظ�
        DeletePost = 0x0000000000000080,         //ɾ���ظ�
        UserAdmin = 0x0000000000000100,          //�û�����
        RoleAdmin = 0x0000000000000200,          //��ɫ����
        RolePermissionAdmin = 0x0000000000000400,//��ɫ��Ȩ����
        SectionAdmin = 0x0000000000000800,       //��鼰���������
        SectionAdminsAdmin = 0x0000000000001000, //��������
        CloseThread = 0x0000000000002000,        //����
        ExceptionLogAdmin = 0x0000000000004000,  //������־����
    }
}
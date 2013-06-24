namespace System.Web.Core
{
    public class RegularExpressions
    {
        public static readonly string QQ_NO = "[1-9][0-9]{4,}";//QQ
        public static readonly string EMAIL = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";//MSN �� EMail
        public static readonly string TELEPHONE = @"\d{2,5}-\d{7,8}(-\d{1,})?";//�̶��绰��С��ͨ (ƥ����ʽ�� 0511-4405222 �� 021-87888822)
        public static readonly string MOBILE = @"^1[3,5]\d{9}$";//�ֻ� 
        public static readonly string IDENTITYCARD = @"(^\d{15}$)|(^\d{17}([0-9]|X)$)";//���֤��    (�й������֤Ϊ15λ��18λ)
        public static readonly string POSTCODE = @"[1-9]\d{5}(?!\d)";//��������     (�й���������Ϊ6λ����)
        public static readonly string CHINESE = "[\u4e00-\u9fa5]";//�����ַ�
        public static readonly string DOUBLE_BYTE = "[^\x00-\xff]";//˫�ֽ��ַ�  (������������)(һ��˫�ֽ��ַ����ȼ�2��ASCII�ַ���1)
        public static readonly string HTML = @"<(\S*?)[^>]*>.*?</\1>|<.*? />";//HTML���    (���Ҳ������ƥ�䲿�֣����ڸ��ӵ�Ƕ�ױ����������Ϊ��)
        public static readonly string URL = @"[a-zA-z]+://[^\s]*";//��ַURL
        public static readonly string IP = @"\d+\.\d+\.\d+\.\d+";//ip��ַ
        public static readonly string ENGLISH_CHAR = @"^[A-Za-z]+$";����//ƥ����26��Ӣ����ĸ��ɵ��ַ���
        public static readonly string UPCHAR = @"^[A-Z]+$";����//ƥ����26��Ӣ����ĸ�Ĵ�д��ɵ��ַ���
        public static readonly string LOWERCHAR = @"^[a-z]+$";����//ƥ����26��Ӣ����ĸ��Сд��ɵ��ַ���
        public static readonly string CHAR_ANDN_NUMBER = @"^[A-Za-z0-9]+$";��//ƥ�������ֺ�26��Ӣ����ĸ��ɵ��ַ���
        public static readonly string SPECIAL_STRING = @"^\w+$";����//ƥ�������֡�26��Ӣ����ĸ�����»�����ɵ��ַ���
        public static readonly string POSITIVE_INTEGERS = @"^[1-9]\d*$";�� �� //ƥ��������
        public static readonly string NEGATIVE_INTEGERS = @"^-[1-9]\d*$"; �� //ƥ�为����
        public static readonly string INTEGERS = @"^-?[1-9]\d*$";���� //ƥ������
        public static readonly string NO_NEGATIVE_INTEGERS = @"^[1-9]\d*|0$";�� //ƥ��Ǹ������������� + 0��
        public static readonly string NO_POSITIVE_INTEGERS = @"^-[1-9]\d*|0$";���� //ƥ����������������� + 0��
        public static readonly string POSITIVE_FLOATS = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$";���� //ƥ����������
        public static readonly string NEGATIVE_FLOATS = @"^-([1-9]\d*\.\d*|0\.\d*[1-9]\d*)$";�� //ƥ�为������
        public static readonly string FLOATS = @"^-?([1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0)$";�� //ƥ�両����
        public static readonly string NO_NEGATIVE_FLOATS = @"^[1-9]\d*\.\d*|0\.\d*[1-9]\d*|0?\.0+|0$";���� //ƥ��Ǹ����������������� + 0��
        public static readonly string NO_POSITIVE_FLOATS = @"^(-([1-9]\d*\.\d*|0\.\d*[1-9]\d*))|0?\.0+|0$";����//ƥ��������������������� + 0��
    }
}

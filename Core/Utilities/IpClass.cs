using System;
using System.IO;
using System.Web;

namespace System.Web.Core
{
    public class IpClass
    {
        private static object lockHelper = new object();

        static PHCZIP pcz = new PHCZIP();

        static string filePath = "";

        static bool fileIsExsit = true;

        static IpClass()
        {
            filePath = HttpContext.Current.Server.MapPath("~/ipentity.config");
            pcz.SetDbFilePath(filePath);
        }

        /// <summary>
        /// 获得客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            // 穿过代理服务器取远程用户真实IP地址
            string ip = string.Empty;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                if (HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
                {
                    if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                    {
                        ip = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                    }
                    else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
                    {
                        ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                    }
                }
                else
                {
                    ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
            }
            else if (HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null)
            {
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return ip;
        }

        /// <summary>
        /// 返回IP查找结果
        /// </summary>
        /// <param name="IPValue">要查找的IP地址</param>
        /// <returns></returns>
        public static string GetAddressWithIP(string IPValue)
        {
            lock (lockHelper)
            {
                string result = pcz.GetAddressWithIP(IPValue.Trim());

                if (fileIsExsit)
                {
                    if (result.IndexOf("IANA") >= 0)
                    {
                        return "";
                    }
                    return result;
                }
                return null;
            }
        }

        /// <summary>
        /// 辅助类，用于保存IP索引信息
        /// </summary>
        ///     
        public class CZ_INDEX_INFO
        {
            public UInt32 IpSet;
            public UInt32 IpEnd;
            public UInt32 Offset;

            public CZ_INDEX_INFO()
            {
                IpSet = 0;
                IpEnd = 0;
                Offset = 0;
            }
        }

        //读取纯真IP数据库类
        public class PHCZIP
        {
            protected bool bFilePathInitialized;
            protected string filePath;
            protected FileStream fileStrm;
            protected UInt32 index_Set;
            protected UInt32 index_End;
            protected UInt32 index_Count;
            protected UInt32 search_Index_Set;
            protected UInt32 search_Index_End;
            protected CZ_INDEX_INFO search_Set;
            protected CZ_INDEX_INFO search_Mid;
            protected CZ_INDEX_INFO search_End;

            public PHCZIP()
            {
                bFilePathInitialized = false;
            }

            public PHCZIP(string dbFilePath)
            {
                bFilePathInitialized = false;
                SetDbFilePath(dbFilePath);
            }

            //使用二分法查找索引区，初始化查找区间
            public void Initialize()
            {
                search_Index_Set = 0;
                search_Index_End = index_Count - 1;
            }

            //关闭文件
            public void Dispose()
            {
                if (bFilePathInitialized)
                {
                    bFilePathInitialized = false;
                    fileStrm.Close();
                    //FileStrm.Dispose();
                }

            }


            public bool SetDbFilePath(string dbFilePath)
            {
                if (dbFilePath.Length == 0)
                {
                    return false;
                }

                try
                {
                    fileStrm = new FileStream(dbFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                }
                catch
                {
                    return false;
                }
                //检查文件长度
                if (fileStrm.Length < 8)
                {
                    fileStrm.Close();
                    //FileStrm.Dispose();
                    return false;
                }
                //得到第一条索引的绝对偏移和最后一条索引的绝对偏移
                fileStrm.Seek(0, SeekOrigin.Begin);
                index_Set = GetUInt32();
                index_End = GetUInt32();

                //得到总索引条数
                index_Count = (index_End - index_Set) / 7 + 1;
                bFilePathInitialized = true;

                return true;

            }

            public string GetAddressWithIP(string iPValue)
            {
                if (!bFilePathInitialized)
                {
                    return "";
                }

                Initialize();

                UInt32 ip = IPToUInt32(iPValue);

                while (true)
                {

                    //首先初始化本轮查找的区间

                    //区间头
                    search_Set = IndexInfoAtPos(search_Index_Set);
                    //区间尾
                    search_End = IndexInfoAtPos(search_Index_End);

                    //判断IP是否在区间头内
                    if (ip >= search_Set.IpSet && ip <= search_Set.IpEnd)
                        return ReadAddressInfoAtOffset(search_Set.Offset);


                    //判断IP是否在区间尾内
                    if (ip >= search_End.IpSet && ip <= search_End.IpEnd)
                        return ReadAddressInfoAtOffset(search_End.Offset);

                    //计算出区间中点
                    search_Mid = IndexInfoAtPos((search_Index_End + search_Index_Set) / 2);

                    //判断IP是否在中点
                    if (ip >= search_Mid.IpSet && ip <= search_Mid.IpEnd)
                        return ReadAddressInfoAtOffset(search_Mid.Offset);

                    //本轮没有找到，准备下一轮
                    if (ip < search_Mid.IpSet)
                        //IP比区间中点要小，将区间尾设为现在的中点，将区间缩小1倍。
                        search_Index_End = (search_Index_End + search_Index_Set) / 2;
                    else
                        //IP比区间中点要大，将区间头设为现在的中点，将区间缩小1倍。
                        search_Index_Set = (search_Index_End + search_Index_Set) / 2;
                }

            }


            private string ReadAddressInfoAtOffset(UInt32 offset)
            {
                string country = "";
                string area = "";
                UInt32 country_Offset = 0;
                byte tag = 0;
                //跳过4字节，因这4个字节是该索引的IP区间上限。
                fileStrm.Seek(offset + 4, SeekOrigin.Begin);

                //读取一个字节，得到描述国家信息的“寻址方式”
                tag = GetTag();

                if (tag == 0x01)
                {

                    //模式0x01，表示接下来的3个字节是表示偏移位置
                    fileStrm.Seek(GetOffset(), SeekOrigin.Begin);

                    //继续检查“寻址方式”
                    tag = GetTag();
                    if (tag == 0x02)
                    {
                        //模式0x02，表示接下来的3个字节代表国家信息的偏移位置
                        //先将这个偏移位置保存起来，因为我们要读取它后面的地区信息。
                        country_Offset = GetOffset();
                        //读取地区信息（注：按照Luma的说明，好像没有这么多种可能性，但在测试过程中好像有些情况没有考虑到，
                        //所以写了个ReadArea()来读取。
                        area = ReadArea();
                        //读取国家信息
                        fileStrm.Seek(country_Offset, SeekOrigin.Begin);
                        country = ReadString();
                    }
                    else
                    {
                        //这种模式说明接下来就是保存的国家和地区信息了，以'\0'代表结束。
                        fileStrm.Seek(-1, SeekOrigin.Current);
                        country = ReadString();
                        area = ReadArea();

                    }
                }
                else if (tag == 0x02)
                {
                    //模式0x02，说明国家信息是一个偏移位置
                    country_Offset = GetOffset();
                    //先读取地区信息
                    area = ReadArea();
                    //读取国家信息
                    fileStrm.Seek(country_Offset, SeekOrigin.Begin);
                    country = ReadString();
                }
                else
                {
                    //这种模式最简单了，直接读取国家和地区就OK了
                    fileStrm.Seek(-1, SeekOrigin.Current);
                    country = ReadString();
                    area = ReadArea();

                }
                //return country + " " + area;
                return country;
            }

            private UInt32 GetOffset()
            {
                byte[] tempByte4 = new byte[4];
                tempByte4[0] = (byte)fileStrm.ReadByte();
                tempByte4[1] = (byte)fileStrm.ReadByte();
                tempByte4[2] = (byte)fileStrm.ReadByte();
                tempByte4[3] = 0;
                return BitConverter.ToUInt32(tempByte4, 0);
            }

            protected string ReadArea()
            {
                byte tag = GetTag();

                if (tag == 0x01 || tag == 0x02)
                {
                    fileStrm.Seek(GetOffset(), SeekOrigin.Begin);
                    return ReadString();
                }
                else
                {
                    fileStrm.Seek(-1, SeekOrigin.Current);
                    return ReadString();
                }
            }

            protected string ReadString()
            {
                UInt32 offset = 0;
                byte[] tempByteArray = new byte[256];
                tempByteArray[offset] = (byte)fileStrm.ReadByte();
                while (tempByteArray[offset] != 0x00)
                {
                    offset += 1;
                    tempByteArray[offset] = (byte)fileStrm.ReadByte();
                }
                return System.Text.Encoding.Default.GetString(tempByteArray).TrimEnd('\0');
            }

            protected byte GetTag()
            {
                return (byte)fileStrm.ReadByte();
            }

            protected CZ_INDEX_INFO IndexInfoAtPos(UInt32 index_Pos)
            {
                CZ_INDEX_INFO index_Info = new CZ_INDEX_INFO();
                //根据索引编号计算出在文件中在偏移位置
                fileStrm.Seek(index_Set + 7 * index_Pos, SeekOrigin.Begin);
                index_Info.IpSet = GetUInt32();
                index_Info.Offset = GetOffset();
                fileStrm.Seek(index_Info.Offset, SeekOrigin.Begin);
                index_Info.IpEnd = GetUInt32();

                return index_Info;
            }

            /// <summary>
            /// 从IP转换为Int32
            /// </summary>
            /// <param name="IpValue"></param>
            /// <returns></returns>
            public UInt32 IPToUInt32(string ipValue)
            {
                string[] ipByte = ipValue.Split('.');
                Int32 nUpperBound = ipByte.GetUpperBound(0);
                if (nUpperBound != 3)
                {
                    ipByte = new string[4];
                    for (Int32 i = 1; i <= 3 - nUpperBound; i++)
                        ipByte[nUpperBound + i] = "0";
                }

                byte[] tempByte4 = new byte[4];
                for (Int32 i = 0; i <= 3; i++)
                {
                    if (IsNumeric(ipByte[i]))
                        tempByte4[3 - i] = (byte)(Convert.ToInt32(ipByte[i]) & 0xff);
                }

                return BitConverter.ToUInt32(tempByte4, 0);
            }

            /// <summary>
            /// 判断是否为数字
            /// </summary>
            /// <param name="str">待判断字符串</param>
            /// <returns></returns>
            protected bool IsNumeric(string str)
            {
                if (str != null && System.Text.RegularExpressions.Regex.IsMatch(str, @"^-?\d+$"))
                    return true;
                else
                    return false;
            }

            protected UInt32 GetUInt32()
            {
                byte[] tempByte4 = new byte[4];
                fileStrm.Read(tempByte4, 0, 4);
                return BitConverter.ToUInt32(tempByte4, 0);
            }
        }
    }
}

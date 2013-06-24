using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Text;
using System.Security.Cryptography;
using System.Configuration.Provider;
using System.Reflection;
using System.Web.Configuration;

namespace System.Web.Core
{
    public class MemberManager
    {
        #region Private Members

        private static EntityProvider memberProvider = Configuration.Instance.Providers["MemberProvider"] as EntityProvider;
        private static string anonymousUserCookieName = "AnonymousUserName";
        private static string loginUserCookieName = "MemberName";

        #endregion

        #region Public Methods

        public static string GetCurrentLoggedOnMemberName()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                Member member = GetFirstMemberByMemberId(HttpContext.Current.User.Identity.Name);
                if (member != null)
                {
                    return member.MemberName.Value;
                }
            }
            return "Anonymous";
        }
        public static Member GetMember(string memberName)
        {
            TRequest<Member> request = new TRequest<Member>();
            request.Data.MemberName.Value = memberName;
            return Engine.GetSingle<Member>(request, memberProvider);
        }
        public static Member GetFirstMemberByMemberId(string memberId)
        {
            TRequest<Member> request = new TRequest<Member>();
            request.Data.MemberId.Value = new Guid(memberId);
            return Engine.GetSingle<Member>(request, memberProvider);
        }
        public static TEntityList<Member> GetMembersByMemberId(Guid memberId)
        {
            TRequest<Member> request = new TRequest<Member>();
            request.Data.MemberId.Value = memberId;
            return Engine.GetAll<Member>(request, memberProvider);
        }
        public static TEntityList<Member> GetMembersByMemberName(string memberName)
        {
            TRequest<Member> request = new TRequest<Member>();
            request.Data.MemberName.Value = memberName;
            request.Data.MemberName.Condition = new Condition(typeof(StringValidator), "Like", memberName);
            return Engine.GetAll<Member>(request, memberProvider);
        }
        public static void Create(Member member, MemberInfo memberInfo)
        {
            if (GetMember(member.MemberName.Value) == null)
            {
                member.PasswordFormat.Value = 1; // 0:Clear, 1:Hashed, 2:Encrypted
                //member.PasswordSalt.Value = GenerateSalt(); not used again.
                member.Password.Value = GetMD5Hash(member.Password.Value);
                member.PasswordSalt.Value = string.Empty;
                Engine.Create(member, memberProvider);
                CreateSiteUsers(memberInfo);
            }
        }
        public static void Delete(Guid memberId, string memberName)
        {
            TRequest<Member> request = new TRequest<Member>();
            request.Data.MemberId.Value = memberId;
            request.Data.MemberName.Value = memberName;
            Engine.DeleteList(request, memberProvider);
            DeleteSiteUsers(memberId);
        }
        public static void UpdateMemberProperties(Guid memberId, string attributeName, string attributeValue)
        {
            foreach (ISite site in Configuration.Instance.Sites.Values)
            {
                site.UpdateUserProperty(memberId, attributeName, attributeValue);
            }
        }
        public static bool ValidateMember(Guid memberId, string password)
        {
            TEntityList<Member> members = GetMembersByMemberId(memberId);
            if (members.Count > 0)
            {
                return members[0].Password.Value == GetMD5Hash(password);
            }
            return false;
        }
        public static bool ValidateMember(string memberName, string password)
        {
            Member member = GetMember(memberName);
            if (member != null)
            {
                return member.Password.Value == GetMD5Hash(password);
            }
            return false;
        }
        public static void ChangeMemberPassword(Member member, string newPassword)
        {
            TRequest<Member> request = new TRequest<Member>();
            request.Data.MemberId.Value = member.MemberId.Value;
            request.UpdatePropertyEntryList.Add(new UpdatePropertyEntry("Password", GetMD5Hash(newPassword)));
            Engine.UpdateList(request, memberProvider);
        }
        public static string ResetMemberPassword(Member member)
        {
            if (member == null)
            {
                return null;
            }

            string newPassword = GeneratePassword(8);
            ChangeMemberPassword(member, newPassword);

            return newPassword;
        }

        public static void Login(string memberName)
        {
            Member member = MemberManager.GetMember(memberName);
            if (member != null)
            {
                CookieManager.AddCookieToResponse(FormsAuthentication.GetAuthCookie(member.MemberId.Value.ToString(), false));
                CookieManager.AddCookieToResponse(new HttpCookie(loginUserCookieName, member.MemberName.Value));
            }
        }
        public static void Login(string memberName, int expireDays)
        {
            Member member = MemberManager.GetMember(memberName);
            if (member != null)
            {
                CookieManager.AddCookieToResponse(FormsAuthentication.GetAuthCookie(member.MemberId.Value.ToString(), false), expireDays);
                CookieManager.AddCookieToResponse(new HttpCookie(loginUserCookieName, member.MemberName.Value), expireDays);
            }
        }
        public static void Logout()
        {
            if (HttpContext.Current.Request.IsAuthenticated)
            {
                FormsAuthentication.SignOut();
                CookieManager.DeleteCookie(FormsAuthentication.GetAuthCookie(HttpContext.Current.User.Identity.Name, false).Name);
                CookieManager.DeleteCookie("MemberName");
            }
        }

        public static void RemoveAnonymousMember()
        {
            CookieManager.DeleteCookie(anonymousUserCookieName);
        }
        public static Guid GetAnonymousMemberId()
        {
            Guid memberId = Guid.Empty;

            if (CookieManager.IsCookieExist(HttpContext.Current.Request.Cookies, anonymousUserCookieName))
            {
                string cookieValue = CookieManager.GetCookieValue(anonymousUserCookieName);
                try
                {
                    memberId = new Guid(cookieValue);
                }
                catch
                {
                    memberId = Guid.NewGuid();
                    CookieManager.SaveCookie(anonymousUserCookieName, memberId.ToString(), 20);
                }
            }
            else
            {
                memberId = Guid.NewGuid();
                CookieManager.SaveCookie(anonymousUserCookieName, memberId.ToString(), 20);
            }

            return memberId;
        }

        public static string GetMD5Hash(string password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            bytes = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2").ToUpper());
            }
            return sb.ToString();
        }
        public static string EncodePassword(string pass, int passwordFormat, string salt)
        {
            if (passwordFormat == 0)
            {
                return pass;
            }
            byte[] bIn = Encoding.Unicode.GetBytes(pass);
            byte[] bSalt = Convert.FromBase64String(salt);
            byte[] bAll = new byte[bSalt.Length + bIn.Length];
            byte[] bRet = null;

            Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
            Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
            if (passwordFormat == 1)
            {
                HashAlgorithm s = HashAlgorithm.Create(Membership.HashAlgorithmType);
                bRet = s.ComputeHash(bAll);
            }
            else if (passwordFormat == 2)
            {
                bRet = EncryptPassword(bAll);
            }
            else
            {
                throw new ProviderException("Invalid password format.");
            }
            return Convert.ToBase64String(bRet);
        }
        public static string UnEncodePassword(string pass, int passwordFormat)
        {
            switch (passwordFormat)
            {
                case 0:
                    return pass;
                case 1:
                    throw new ProviderException("Can not decode hashed password.");
                case 2:
                    byte[] bIn = Convert.FromBase64String(pass);
                    byte[] bRet = DecryptPassword(bIn);
                    if (bRet == null)
                        return null;
                    return Encoding.Unicode.GetString(bRet, 16, bRet.Length - 16);
                default:
                    throw new ProviderException("Invalid password format.");
            }
        }
        public static byte[] DecryptPassword(byte[] encodedPassword)
        {
            Type machineKeySection = typeof(MachineKeySection);
            Type[] paramTypes = new Type[] { typeof(bool), typeof(byte[]), typeof(byte[]), typeof(int), typeof(int) };
            MethodInfo encryptOrDecryptData = machineKeySection.GetMethod("EncryptOrDecryptData", BindingFlags.Static | BindingFlags.NonPublic, null, paramTypes, null);

            return (byte[])encryptOrDecryptData.Invoke(null, new object[] { false, encodedPassword, null, 0, encodedPassword.Length });
        }
        public static byte[] EncryptPassword(byte[] password)
        {
            Type machineKeySection = typeof(MachineKeySection);
            Type[] paramTypes = new Type[] { typeof(bool), typeof(byte[]), typeof(byte[]), typeof(int), typeof(int) };
            MethodInfo encryptOrDecryptData = machineKeySection.GetMethod("EncryptOrDecryptData", BindingFlags.Static | BindingFlags.NonPublic, null, paramTypes, null);

            return (byte[])encryptOrDecryptData.Invoke(null, new object[] { true, password, null, 0, password.Length });
        }
        public static string GenerateSalt()
        {
            byte[] entity = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(entity);
            return Convert.ToBase64String(entity);
        }
        private static int rep = 0;
        public static string GeneratePassword(int length)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0; i < length; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            return str;
        }

        #endregion

        #region Private Methods

        private static void CreateSiteUsers(MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                return;
            }
            foreach (ISite site in Configuration.Instance.Sites.Values)
            {
                site.CreateUser(memberInfo);
            }
        }
        private static void DeleteSiteUsers(Guid memberId)
        {
            foreach (ISite site in Configuration.Instance.Sites.Values)
            {
                site.DeleteUser(memberId);
            }
        }

        #endregion
    }
}
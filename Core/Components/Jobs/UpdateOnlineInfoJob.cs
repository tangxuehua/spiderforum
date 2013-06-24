using System.Xml;

namespace System.Web.Core
{
    public class UpdateOnlineUsersJob : IJob
    {
        public void Execute(XmlNode node)
        {
            OnlineMemberManager.Instance.RefreshAllUsers(Configuration.Instance.UserOnlineTimeWindow);
        }
    }
}

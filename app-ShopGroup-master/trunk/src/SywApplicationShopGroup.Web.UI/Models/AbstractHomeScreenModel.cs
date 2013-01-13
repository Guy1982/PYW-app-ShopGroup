using System.Collections.Generic;
using SywApplicationShopGroup.Domain.Entities;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public abstract class AbstractHomeScreenModel
    {
        public enum PageId
        {
            MyGroups,
            CreateAnewGroup,
            JoinAgroup,
            HowDoesItWork

        }
        public abstract IList<ShopGroup> GetGroups();
        public abstract long UserId();
        public abstract string SessionToken();
        public abstract PageId GetPageId();
    }
}

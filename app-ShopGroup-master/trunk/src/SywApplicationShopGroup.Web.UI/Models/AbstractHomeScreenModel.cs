using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SywApplicationShopGroup.Domain.Entities;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public abstract class AbstractHomeScreenModel
    {
        public abstract IList<ShopGroup> GetGroups();
        public abstract long UserId();
        public abstract string SessionToken();
    }
}

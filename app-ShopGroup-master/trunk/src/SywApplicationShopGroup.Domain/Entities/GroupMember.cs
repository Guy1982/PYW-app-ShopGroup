using System.Collections.Generic;

namespace SywApplicationShopGroup.Domain.Entities
{
    public class GroupMember
    {
        public virtual long SywId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Token { get; set; }
        public virtual IList<ShopGroup> Groups { get; set; }

        public GroupMember()
        {
           Groups = new List<ShopGroup>();
        }


    }
}

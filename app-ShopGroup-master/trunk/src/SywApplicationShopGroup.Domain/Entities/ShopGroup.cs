using System;
using System.Collections.Generic;

namespace SywApplicationShopGroup.Domain.Entities
{
    public enum GroupState
    {
        Open,
        Buying,
        Closed

    };
    public class ShopGroup
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual long ProductId { get; set; }
        public virtual GroupState Status { get; set; }
        public virtual DateTime CreationTime { get; set; }
        public virtual DateTime StartBuyingTime { get; set; }
        public virtual int BuyingDration { get; set; }
        public virtual IList<GroupMember> Members { get; set; }

        public virtual GroupMember Admin
        {
            get { return _admin; }
            set { _admin = value; AddMember(value); }
        }
        private GroupMember _admin;
   


        public ShopGroup()
        {
            Members = new List<GroupMember>();
            CreationTime = DateTime.UtcNow;
        }

        public virtual void AddMember(GroupMember member)
        {
            if (Members.Contains(member)) return;

            Members.Add(member);
            member.Groups.Add(this);
        }

    }
}

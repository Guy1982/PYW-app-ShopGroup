using FluentNHibernate.Mapping;
using SywApplicationShopGroup.Domain.Entities;

namespace SywApplicationShopGroup.Domain.Mappings
{
    public class GroupMemberMap : ClassMap<GroupMember>
    {
        public GroupMemberMap()
        {
            Id(x => x.SywId).GeneratedBy.Assigned();
            Map(x => x.Name);
            Map(x => x.Token);
            HasManyToMany(x => x.Groups)
              .Cascade.All()
              .Inverse()
              .Table("shopgroup_groupmember");         
        }
    }
}

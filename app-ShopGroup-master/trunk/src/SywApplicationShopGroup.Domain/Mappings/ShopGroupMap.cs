using System;
using FluentNHibernate.Mapping;
using SywApplicationShopGroup.Domain.Entities;


namespace SywApplicationShopGroup.Domain.Mappings
{
    public class ShopGroupMap : ClassMap<ShopGroup>
    {
        public ShopGroupMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.ProductId);
            Map(x => x.Status);
            Map(x => x.CreationTime);
            Map(x => x.StartBuyingTime);
            Map(x => x.BuyingDration);
            References(x => x.Admin);
            HasManyToMany(x => x.Members)
            .Cascade.All()
            .Table("shopgroup_groupmember");
            
        }
    }
}
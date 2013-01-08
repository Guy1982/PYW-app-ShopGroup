using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SywApplicationShopGroup.Domain.Entities;
using SywApplicationShopGroup.Domain.Products;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public class ShopGroupHomeScreenModel
    {
        public long Id { get; set; }
        public string MemberName { get; set; }
        public string ImageUrl { get; set; }
        public Uri ProfileUrl { get; set; }
        public IList<ShopGroupHomeScreenModel> Followers { get; set; }
        public IList<ShopGroupHomeScreenModel> Following { get; set; }
        public IList<ShopGroup> ShopGroups { get; set; }
        public ProductDto Product { get; set; }

    }
}
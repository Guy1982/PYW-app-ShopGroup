﻿using SywApplicationShopGroup.Domain.Auth;

namespace SywApplicationShopGroup.Web.UI.Models
{
    public class ProductViewModel
    {
        public string UserName { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public string SessionToken { get; set; }

      
    }
}
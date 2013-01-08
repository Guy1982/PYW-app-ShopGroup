using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SywApplicationShopGroup.Web.UI.Controllers
{
    public interface IShopGroupFromInputValidator
    {
        bool IsInputValid(FormCollection formValue);
        string GetToken(FormCollection formValue);
        string GetGroupName(FormCollection formValue);
        long GetProductId(FormCollection formValue);
    }

    public class ShopGroupFromInputValidator : IShopGroupFromInputValidator
    {
        public bool IsInputValid(FormCollection formValue)
        {
            var tokenStr = formValue["token"];
            var groupName = formValue["txtGroupName"];
            var productIdStr = formValue["productId"];

            try
            {
                var id = Convert.ToInt32(productIdStr);
            }
            catch
            {
                return false;
            }

            return tokenStr != null && groupName != null && groupName.Any();
        }

        public string GetToken(FormCollection formValue)
        {
            return formValue["token"];
        }

        public string GetGroupName(FormCollection formValue)
        {
            return formValue["txtGroupName"];
        }

        public long GetProductId(FormCollection formValue)
        {
            return Convert.ToInt32(formValue["productId"]);
        }

    }


}
using System;
using System.Web.Mvc;

namespace SywApplicationShopGroup.Domain.Repositorys
{
    public enum InputFiled
    {
        Token,
        GroupName,
        GroupId,
        ProductId,
        UserId
    }
    public interface IShopGroupFromInputValidator
    {
        bool IsInputValid(FormCollection formValue, InputFiled[] keys);
        string GetStringKey(InputFiled key, FormCollection formValue);
        long GetLongKey(InputFiled key, FormCollection formValue);
    }

    public class ShopGroupFromInputValidator : IShopGroupFromInputValidator
    {
       
        public bool IsInputValid(FormCollection formValue, InputFiled[] keys)
        {
            foreach (var inputFiled in keys)
            {
                if (formValue[GetKeyValue(inputFiled)] == null )
                {
                    return false;
                }

                if(inputFiled.ToString().EndsWith("Id"))
                {
                    try
                    {
                        //Testing the convertion
                        Convert.ToInt32(formValue[GetKeyValue(inputFiled)]);
                    }
                    catch
                    {
                        return false;
                    }
                }            
            }

            return true;
        }
        public string GetStringKey(InputFiled key, FormCollection formValue)
        {
            return formValue[GetKeyValue(key)];
        }
        public long GetLongKey(InputFiled key, FormCollection formValue)
        {
            try
            {            
                return Convert.ToInt32(formValue[GetKeyValue(key)]);
            }
            catch
            {
                return 0;
            }
        }

        private string GetKeyValue(InputFiled key)
        {
            switch (key)
            {
                case InputFiled.UserId:
                    return "userId";
                case InputFiled.GroupId:
                    return "groupId";
                case InputFiled.GroupName:
                    return "txtGroupName";
                case InputFiled.ProductId:
                    return "productId";
                case InputFiled.Token:
                    return "token";
                default:
                    return "";
            }
        }
    }


}
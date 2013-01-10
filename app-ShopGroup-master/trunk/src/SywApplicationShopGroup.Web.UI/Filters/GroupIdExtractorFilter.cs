using System;
using System.Linq;
using System.Web.Mvc;
using Platform.Client;
using Platform.Client.Common.Context;

namespace SywApplicationShopGroup.Web.UI.Filters
{
	public class GroupIdExtractorFilter : ActionFilterAttribute
	{
		private readonly IGroupIdProvider _groupIdProvider;

        public GroupIdExtractorFilter()
		{
            _groupIdProvider = new GroupIdProvider(new HttpContextProvider()); 
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);
            //.HttpContext.Request.QueryString["token"];
		    var origianlUri = filterContext.HttpContext.Request.UrlReferrer;
            if(origianlUri == null) return;

	       var queryString = string.Join(string.Empty, origianlUri.ToString().Split('?').Skip(1));
           var dic = System.Web.HttpUtility.ParseQueryString(queryString);
          
            if (dic.Get("groupId") == null)
               return;

            var groupId = dic.Get("groupId");
            try
            {
                //Testing the convertion
                _groupIdProvider.Set(Convert.ToInt32(groupId));
            }
            catch
            {
                
            }
		}
	}
}

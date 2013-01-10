using System;
using Platform.Client.Common.Context;


namespace SywApplicationShopGroup.Domain.WallPublish
{
	public interface IWallPublishApi
	{
	    bool PublishStory(string postTitle, string postContent, string imageUrl);
        bool PublishPromotionalStory(string postTitle, string postContent, string entityId, string imageUrl);
	}

    public class WallPublishApi : ApiBase, IWallPublishApi
	{

        public WallPublishApi(IContextProvider contextProvider):base(contextProvider)
        {
        }

        public bool PublishStory(string postTitle, string postContent, string imageUrl)
        {    
           
            try
            {
                 Proxy.Get<string>(GetEndpointPath("publish-user-action"),
                                                     new
                                                     {
                                                         UserAction = "shared this",
                                                         Title = postTitle,
                                                         Content = postContent,
                                                         ImageUrl = imageUrl ?? ""
                                                     }); 
            }
            catch (Exception e)
            {
                return false;
            }       

            return true;
        }


 
        public bool PublishPromotionalStory(string postTitle, string postContent, string entityId, string imageUrl)
        {
                        
           try
            {
                Proxy.Get<string>(GetEndpointPath("publish-user-action"),
                                                     new
                                                     {
                                                         UserAction = "shared this",
                                                         Title = postTitle,
                                                         Content = postContent,
                                                         ImageUrl = imageUrl ?? "",
                                                         Ids = entityId
                                                     }); 
            }
            catch
            {
                return false;
            }

            return true;
        }

        protected override string BasePath { get { return "wall"; } }
	}
}

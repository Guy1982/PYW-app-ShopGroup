

using Platform.Client.Configuration;

namespace SywApplicationShopGroup.Domain.Configuration
{
	public class ApplicationSettings : IApplicationSettings
	{
		public long AppId { get { return Config.GetLong("app:id"); } }
		public string AppSecret { get { return Config.GetString("app:secret"); } }
        public string AppDbPath { get { return Config.GetString("app:db-name"); } }
	}
}

using Platform.Client.Common.Context;

namespace Platform.Client
{
    public interface IGroupIdProvider
	{
		void Set(int groupId);
		int Get();
	}

    public class GroupIdProvider : IGroupIdProvider
	{
		private const string TokenKey = "groupId";
		private readonly IContextProvider _contextProvider;

        public GroupIdProvider(IContextProvider contextProvider)
		{
			_contextProvider = contextProvider;
		}

		public void Set(int groupId)
		{
			_contextProvider.Set(TokenKey, groupId);
		}

		public int Get()
		{
			return _contextProvider.Get<int>(TokenKey);
		}
	}
}

using System;

namespace SywApplicationShopGroup.Domain
{
    public interface ITimeFactory
    {
        DateTime GetUtcTime();
        DateTime GetLocalTime();
    }

    public class TimeFactory : ITimeFactory
    {
        public DateTime GetUtcTime()
        {
            return DateTime.UtcNow;
        }

        public DateTime GetLocalTime()
        {
            return DateTime.UtcNow;
        }
    }
  
}

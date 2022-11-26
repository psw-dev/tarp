using System;

namespace PSW.ITMS.Common
{
    public interface IRedisCacheService
    {
        dynamic Get<T>(string key);
        bool KeyExists(string key);
        bool Set<T>(string key, T value, TimeSpan expiry);
    }
}
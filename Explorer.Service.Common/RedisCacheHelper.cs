using System;
using System.Text;
using Thor.Framework.Common.Cache;
using Thor.Framework.Common.Helper.Extensions;

namespace Explorer.Service.Common
{
    public static class RedisCacheHelper
    {
        public const int DefaultDatabase = 2;
        
        public static bool TryGetCache<TValue>(string key, out TValue result)
        {
            try
            {
                var value = RedisCacheAdvHelper.GetDatabase(DefaultDatabase).StringGet(key);

                if (typeof(TValue) == typeof(string))
                {
                    result = (TValue) ((object) Encoding.UTF8.GetString(value));
                }
                else
                {
                    result = (TValue) ((byte[]) value).ToObject();
                }

                return true;
            }
            catch (Exception)
            {
                result = default;
                return false;
            }
        }

        public static bool TrySetCache(string key, object data, TimeSpan? expiredTime = null)
        {
            try
            {
                var bytes = data.ToBytes();
                return TrySetCache(key, bytes, expiredTime);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool TrySetCache(string key, string data, TimeSpan? expiredTime = null)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return TrySetCache(key, bytes, expiredTime);
        }

        public static bool TrySetCache(string key, byte[] bytes, TimeSpan? expiredTime = null)
        {
            try
            {
                RedisCacheAdvHelper.GetDatabase(DefaultDatabase).StringSet(key, bytes, expiredTime);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
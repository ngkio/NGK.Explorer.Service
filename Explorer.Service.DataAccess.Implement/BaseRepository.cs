using System;
using Thor.Framework.Data.DbContext.Relational;
using Explorer.Service.Common;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Service.DataAccess.Implement
{
    public class BaseRepository<T> where T : class
    {
        protected readonly IDbContextCore DbContext;
        protected readonly DbSet<T> DbSet;

        public BaseRepository(IDbContextCore dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.GetDbSet<T>();
        }

        protected bool TryGetCache<TValue>(string key, out TValue result)
        {
            return RedisCacheHelper.TryGetCache(key, out result);
        }

        protected bool TrySetCache(string key, object data, TimeSpan? expiredTime = null)
        {
            return RedisCacheHelper.TrySetCache(key, data, expiredTime);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using Explorer.Service.DataAccess.Entities.Enums;
using Explorer.Service.DataAccess.Entities.Extensions;
using Thor.Framework.Common.Helper.Extensions;
using Thor.Framework.Common.Options;
using Thor.Framework.Data.DbContext.Relational;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Explorer.Service.DataAccess.Entities
{
    public partial class ChainContext : IDbContextCore
    {
        static ChainContext()
        {
            NpgsqlConnection.GlobalTypeMapper.MapEnum<EnumTransactionStatus>();
            NpgsqlConnection.GlobalTypeMapper.MapComposite<KeyWeight>();
            NpgsqlConnection.GlobalTypeMapper.MapComposite<WaitWeight>();
            NpgsqlConnection.GlobalTypeMapper.MapComposite<PermissionLevel>();
            NpgsqlConnection.GlobalTypeMapper.MapComposite<AccountWeight>();
        }

        public ChainContext(IOptions<DbContextOption> option) : base(option)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Option.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public IList<T> ExecuteSqlQuery<T>(string sql, params NpgsqlParameter[] parameters) where T : new()
        {
            DbConnection dbConnection = Database.GetDbConnection();
            try
            {
                dbConnection.Open();
                using (DbCommand command = dbConnection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.AddRange(parameters);
                    PropertyInfo[] properties = typeof(T).GetProperties();
                    List<T> objList = new List<T>();
                    using (DbDataReader dbDataReader = command.ExecuteReader())
                    {
                        while (dbDataReader.Read())
                        {
                            T obj1 = new T();
                            foreach (PropertyInfo propertyInfo in properties)
                            {
                                object obj2 = dbDataReader[propertyInfo.Name];
                                if (obj2 == DBNull.Value)
                                {
                                    propertyInfo.SetValue((object)obj1, (object)null);
                                }
                                else if (propertyInfo.PropertyType == typeof(long))
                                {
                                    propertyInfo.SetValue((object)obj1, obj2.ToLong());
                                }
                                else if (propertyInfo.PropertyType == typeof(bool))
                                {
                                    propertyInfo.SetValue((object)obj1, obj2.ToBool());
                                }
                                else if (propertyInfo.PropertyType == typeof(int))
                                {
                                    propertyInfo.SetValue((object)obj1, obj2.ToInt());
                                }
                                else
                                {
                                    propertyInfo.SetValue((object)obj1, obj2);
                                }
                            }

                            objList.Add(obj1);
                        }
                    }

                    return (IList<T>)objList;
                }
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
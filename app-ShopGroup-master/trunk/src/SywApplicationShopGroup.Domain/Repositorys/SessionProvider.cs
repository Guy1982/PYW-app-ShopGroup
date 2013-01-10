using System;
using System.Collections.Generic;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Linq;
using SywApplicationShopGroup.Domain.Entities;

namespace SywApplicationShopGroup.Domain.Repositorys
{
    public interface ISessionProvider
    {
        void WithSession(Action<ISession> action);
        IList<T> Query<T>(Func<IQueryable<T>, IQueryable<T>> query);
        IList<T1> QueryOver<T, T1>(Func<IQueryable<T>, IQueryable<T1>> query);

    }
    public class SessionProvider : ISessionProvider
    {
        public static readonly ISessionFactory SessionFactory = Create();

        private static ISessionFactory Create()
        {
            return Fluently.Configure()
                .Database(MySQLConfiguration.Standard.ShowSql().ConnectionString(x => x.FromConnectionStringWithKey("MySql")))
                .Mappings(x => x.FluentMappings.AddFromAssemblyOf<ShopGroup>().Conventions.Add(FluentNHibernate.Conventions.Helpers.DefaultLazy.Never()))
                .BuildSessionFactory();
        }

        public void WithSession(Action<ISession> action)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    action(session);
                    session.Flush();
                    transaction.Commit();
                }
            }
        }

        public IList<T> Query<T>(Func<IQueryable<T>, IQueryable<T>> query)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var queryResult = query(session.Query<T>()).ToList();
                    transaction.Commit();
                    return queryResult;

                }
            }
        }

        public IList<T1> QueryOver<T, T1>(Func<IQueryable<T>, IQueryable<T1>> query)
        {
            using (var session = SessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    var queryResult = query(session.Query<T>()).ToList<T1>();
                    transaction.Commit();                 
                    return queryResult;

                }
            }
        }
    }
}

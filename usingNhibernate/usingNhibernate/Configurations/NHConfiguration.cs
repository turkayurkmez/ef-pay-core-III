using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Reflection;

namespace usingNhibernate.Configurations
{
    public class NHConfiguration
    {
        private readonly Configuration configuration;
        public NHConfiguration()
        {
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=NHSampleDb;Integrated Security=True;";
            configuration = new Configuration();
            configuration.DataBaseIntegration(config =>
            {
                config.ConnectionString = connectionString;
                config.Driver<SqlClientDriver>();
                config.Dialect<MsSql2012Dialect>();
            });

            configuration.AddAssembly(Assembly.GetExecutingAssembly());
        }

        public ISessionFactory GetSessionFactory()
        {
            return configuration.BuildSessionFactory();
        }
    }
}

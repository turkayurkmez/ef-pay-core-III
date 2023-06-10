using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using System.Reflection;
using usingNhibernate.Mapping.FM;

namespace usingNhibernate.Configurations
{
    public class NHibernateConfiguration
    {
        private readonly Configuration configuration;
        public NHibernateConfiguration()
        {
            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=nhiberSampleDb;Integrated Security=True;";
            configuration = new Configuration();
            configuration.DataBaseIntegration(config =>
            {
                config.ConnectionString = connectionString;
                config.Driver<SqlClientDriver>();
                config.Dialect<MsSql2012Dialect>();
            }).AddMapping(getMappings());

            configuration.AddAssembly(Assembly.GetExecutingAssembly());

        }

        private HbmMapping getMappings()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<AddressMapping>();
            mapper.AddMapping<EmployeeMapping>();
            mapper.AddMapping<BenefitMapping>();
            mapper.AddMapping<LeaveBenefitMapping>();
            mapper.AddMapping<FoodTicketMapping>();

            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }

        public ISessionFactory GetSessionFactory()
        {
            return configuration.BuildSessionFactory();
        }

        public ISession GetSession()
        {
            var sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}

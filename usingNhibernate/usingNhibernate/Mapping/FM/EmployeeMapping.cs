using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using usingNhibernate.Models;

namespace usingNhibernate.Mapping.FM
{
    public class EmployeeMapping : ClassMapping<Employee>
    {
        public EmployeeMapping()
        {
            Id(e => e.Id, mapper => mapper.Generator(Generators.Identity));
            Property(e => e.Name);
            Property(e => e.LastName);

            //Set(e => e.Benefits, mapper =>
            //{
            //    mapper.Key(k => k.Column("EmployeeId"));
            //    mapper.Cascade(Cascade.All);
            //});

            OneToOne(e => e.Address, mapper =>
            {
                mapper.Cascade(Cascade.Persist);
                mapper.PropertyReference(a => a.Employee);
            });



        }
    }
}

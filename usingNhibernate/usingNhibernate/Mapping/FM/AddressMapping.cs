using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using usingNhibernate.Models;

namespace usingNhibernate.Mapping.FM
{
    public class AddressMapping : ClassMapping<Address>
    {
        public AddressMapping()
        {
            Id(a => a.Id, mapper => mapper.Generator(Generators.Identity));
            Property(a => a.AddressLine1);
            Property(a => a.AddressLine2);
            Property(a => a.City);
            Property(a => a.Country);

            ManyToOne(a => a.Employee, _mapper =>
            {
                _mapper.Class(typeof(Employee));
                _mapper.Column("EmployeeId");
                _mapper.Unique(true);
            });



        }
    }
}

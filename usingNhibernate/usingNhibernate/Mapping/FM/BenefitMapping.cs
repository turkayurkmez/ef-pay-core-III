using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using usingNhibernate.Models;

namespace usingNhibernate.Mapping.FM
{
    public class BenefitMapping : ClassMapping<Benefit>
    {
        public BenefitMapping()
        {
            Id(b => b.Id, _mapper => _mapper.Generator(Generators.Identity));
            Property(b => b.Name);
            Property(b => b.Description);

            ManyToOne(b => b.Employee, _mapping =>
            {
                _mapping.Class(typeof(Employee));
                _mapping.Column("EmployeeId");
                _mapping.Unique(true);

            });

        }
    }

    public class LeaveBenefitMapping : JoinedSubclassMapping<LeaveBenefit>
    {
        public LeaveBenefitMapping()
        {
            Key(k => k.Column("Id"));
            Property(l => l.Type);
            Property(l => l.Available);
            Property(l => l.Remaining);


        }
    }

    public class FoodTicketMapping : JoinedSubclassMapping<FoodTicket>
    {
        public FoodTicketMapping()
        {
            Key(k => k.Column("Id"));
            Property(l => l.StartDate);
            Property(l => l.EndDate);
            Property(l => l.Amount);


        }
    }
}

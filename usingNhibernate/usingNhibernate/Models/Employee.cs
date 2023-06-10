namespace usingNhibernate.Models
{
    public class EntiyBase
    {
        public virtual int Id { get; set; }
    }
    public class Employee : EntiyBase
    {
        public virtual string Name { get; set; }
        public virtual string LastName { get; set; }
        public virtual Address Address { get; set; }
        public virtual ICollection<Benefit> Benefits { get; set; }

    }

    public class Address : EntiyBase
    {

        public virtual string City { get; set; }
        public virtual string Country { get; set; }
        public virtual string AddressLine1 { get; set; }
        public virtual string AddressLine2 { get; set; }


        public virtual Employee Employee { get; set; }


    }

    public class Benefit : EntiyBase
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual Employee Employee { get; set; }


    }

    public enum LeaveType
    {
        Casual,
        Sick,
        Unpaid
    }

    public class LeaveBenefit : Benefit
    {
        public virtual LeaveType Type { get; set; }
        public virtual int Available { get; set; }
        public virtual int Remaining { get; set; }

    }

    public class FoodTicket : Benefit
    {
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual decimal Amount { get; set; }
    }

}

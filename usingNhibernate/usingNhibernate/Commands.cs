using NHibernate.Util;
using usingNhibernate.Configurations;
using usingNhibernate.Models;

namespace usingNhibernate
{
    public static class Commands
    {
        public static void CreateStudent()
        {

            Console.Write("Öğrenci adı:");
            string name = Console.ReadLine();
            Console.Write("Öğrenci soyadı:");
            string lastName = Console.ReadLine();

            Student student = new Student() { FirstName = name, LastName = lastName };


            var sessionFactory = new NHibernateConfiguration().GetSessionFactory();
            var session = sessionFactory.OpenSession();
            var transaction = session.BeginTransaction();
            session.Save(student); //dbContext.Students.Add()
            transaction.Commit();

            Console.WriteLine($"{student.FirstName} {student.LastName} isimli öğrenci eklendi. Öğrencinin id'si: {student.Id}");



        }

        public static void GetStudents()
        {
            // var sessionFactory = new NHibernateConfiguration().GetSessionFactory();
            var session = new NHibernateConfiguration().GetSession();
            var transaction = session.BeginTransaction();
            var students = session.CreateCriteria<Student>().List<Student>();
            students.ForEach(s => Console.WriteLine(s.FirstName + " " + s.LastName));

            //criteria ile belleğe aktarılan koleksiyonda güncelleme işlemi yapılabilir
            var student = session.Get<Student>(1);
            Console.WriteLine($"Id'si 1 olan kişi: {student?.FirstName} {student?.LastName}");

            student.LastName = "Yıldız";
            session.Update(student);
            Console.WriteLine("======================= Güncelleme sonrası ==========================");
            students.ForEach(s => Console.WriteLine(s.FirstName + " " + s.LastName));


            session.Delete(student);
            Console.WriteLine("======================= Silme sonrası ==========================");
            //ancak, silme işlemi criteria'ya yansımaz
            students = session.CreateCriteria<Student>().List<Student>();
            students.ForEach(s => Console.WriteLine(s.FirstName + " " + s.LastName));


            //foreach (var item in students)
            //{
            //    var st = item as Student;
            //    Console.WriteLine(st.FirstName + " " + st.LastName);
            //}
            transaction.Commit();


        }



        public static void getEmployeesWithAddress()
        {
            var session = new NHibernateConfiguration().GetSession();
            var tran = session.BeginTransaction();
            var employee = session.Query<Employee>().FirstOrDefault(e => e.Name == "Türkay");
            Console.WriteLine($"{employee.Name} {employee.LastName} isimli çalışanın adresi: {employee.Address.AddressLine1} {employee.Address.AddressLine2} {employee.Address.City}");

            //Console.WriteLine(employee.Benefits.Count);


            tran.Commit();

            tran = session.BeginTransaction();

            var benefit = session.Query<Benefit>().FirstOrDefault(e => e.Id == 1);
            Console.WriteLine($"{benefit.Name} {benefit.Description} {benefit.Employee.Name} {benefit.GetType().Name}");
            //var firstBenefit = employee.Benefits.ToList()[0];
            //Console.WriteLine($"{benefit.Name} {benefit.Description} {benefit.Employee.Name} {benefit.GetType().Name}");


        }
    }
}

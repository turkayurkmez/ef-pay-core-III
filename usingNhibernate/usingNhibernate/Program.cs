using NHibernate.Util;
using usingNhibernate.Configurations;
using usingNhibernate.Models;

namespace usingNhibernate
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //createStudent();
            getStudents();
        }

        private static void getStudents()
        {
            var sessionFactory = new NHConfiguration().GetSessionFactory();
            var session = sessionFactory.OpenSession();
            using var readingTransaction = session.BeginTransaction();
            var students = session.CreateCriteria<Student>().List<Student>();
            //var students = db.Students.ToList();

            students.ForEach(st => Console.WriteLine($"{st.FirstName} \t {st.LastName}"));

            var student = session.Get<Student>(1);
            Console.WriteLine("Id'si 1 olan öğrenci");
            Console.WriteLine($"{student.FirstName} \t {student.LastName}");

            student.LastName = "Yıldız";
            session.Update(student);
            Console.WriteLine("-------------------- Güncelleme sonrası ----------------------------");
            students.ForEach(st => Console.WriteLine($"{st.FirstName} \t {st.LastName}"));

            session.Delete(student);

            Console.WriteLine("-------------------- Silinme sonrası ----------------------------");
            students = session.CreateCriteria<Student>().List<Student>();
            students.ForEach(st => Console.WriteLine($"{st.FirstName} \t {st.LastName}"));

            readingTransaction.Commit();

        }

        static void createStudent()
        {
            var sessionFactory = new NHConfiguration().GetSessionFactory();
            var session = sessionFactory.OpenSession();

            var transaction = session.BeginTransaction();
            Console.Write("Öğrenci Adı: ");
            var name = Console.ReadLine();
            Console.Write("Öğrenci Soyadı: ");
            var lastName = Console.ReadLine();

            var student = new Student { FirstName = name, LastName = lastName };

            session.Save(student);
            transaction.Commit();
            Console.WriteLine($"{student.FirstName} isimli öğrenci eklendi. Yeni id değeri: {student.Id}");



        }
    }
}
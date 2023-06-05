namespace demoEFApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("veritabanı oluşturuluyor.....");
            //Commands.CreateDatabaseAndAfterSeed(true);
            //Console.WriteLine("db oluşturuldu");

            Commands.ListProducts();
            // Commands.AddNewProductToCategory();
            Console.WriteLine("Fiyat değiştiriliyor.....");
            Commands.ChangeProductPrice();
            Commands.ListProducts();


        }
    }
}
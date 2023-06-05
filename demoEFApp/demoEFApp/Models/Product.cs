namespace demoEFApp.Models
{

    /* POCO: Plain Old CLR (C#) Object */
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        //Navigation Property:
        public Category Category { get; set; }


    }
}

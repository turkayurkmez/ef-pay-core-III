namespace Movies.DataApplication.Models
{
    //Bu interface'in amacı generic bir T tipini işaretlemek
    public interface IEntity
    {
        int Id { get; set; }
    }
}

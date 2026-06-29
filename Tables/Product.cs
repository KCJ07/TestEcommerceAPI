// class for Products table
public class Product
{
    public int Id{get; set;}

    public int Amt{get; set;}

    public string Name{get; set;}

    public decimal Price{get; set;}

    // soft delete
    public bool IsDeleted{get; set;} = false;

    // foreign key
    public int CategoryId { get; set; }       
    public Category Category { get; set; }    


    // setting up many to many
    public List<Sale> Sales { get; } = new List<Sale>();

}

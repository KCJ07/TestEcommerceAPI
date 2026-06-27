// class for sales table

public class Sale
{
    public int Id{get; set;}

    // setting up the many to many
    public List<Product> Products { get; } = new List<Product>();

    public int CardType{get; set;}
}

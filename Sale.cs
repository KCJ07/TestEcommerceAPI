// class for sales table

public class Sale()
{
    public int Id{get; set;}

    // setting up the many to many
    public List<Product> Tags { get; } = new List<Product>();


}

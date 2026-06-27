// class for Products table

public class Product()
{
    public int Id{get; set;}

    // setting up many to many
    public List<Sale> Sales { get; } = new List<Sale>();

}

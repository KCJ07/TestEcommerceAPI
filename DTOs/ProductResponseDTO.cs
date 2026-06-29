// Product response class to avoid cyclical calls the the database

public class ProductResponseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Amt { get; set; }
    public string? CategoryName { get; set; }
    public List<int> SaleIds { get; set; } = [];
}
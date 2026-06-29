// Product response class to avoid cyclical calls the the database

public class CategoryResponseDTO
{
    public int Id { get; set; }
    public string Name{get; set;}
    public List<string> ProductNames { get; set; } = [];
}
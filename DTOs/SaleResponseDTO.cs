// Product response class to avoid cyclical calls the the database

public class SaleResponseDTO
{
    public int Id { get; set; }
    public string CardType{get; set;}
    public List<SaleProductDTO> Products { get; set; } = [];
}
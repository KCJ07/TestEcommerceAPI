// class for Products table

using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;

public class Product
{
    public int Id{get; set;}

    public int Amt{get; set;}

    public int Name{get; set;}

    public decimal Price{get; set;}

    // soft delete
    public bool IsDeleted{get; set;} = false;

    // foreign key
    public int CategoryId { get; set; }       
    public required Category Category { get; set; }    


    // setting up many to many
    public List<Sale> Sales { get; } = new List<Sale>();

}

// ============================================================
// EcommerceAPI
// Author: Kevin J.
// Date: June 26, 2026
// ============================================================
// A RESTful Web API for managing an e-commerce platform.
// Built with ASP.NET Core, Entity Framework Core, and SQLite.
//
// Reference: https://thecsharpacademy.com/project/18/ecommerce-api
// ============================================================
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<EcomDb>(opt =>
{
    opt.UseSqlite("Data Source=EcomDb.db");
});

var app = builder.Build();

// create tables in a scoped ENV
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<EcomDb>();
db.Database.EnsureCreated(); 

// GET all products
// pagnation capabilites
// supports soft deletes
app.MapGet("/products/", async (EcomDb db, int page=1, int pageSize=10 ) =>
{
    var products = await db.Products 
    .Where(p => !p.IsDeleted) 
    .Include(p => p.Category)
    .Include(p => p.Sales)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    var totalCount = await db.Products
    .Where(p => !p.IsDeleted)
    .CountAsync();

    PageResult<ProductResponseDto> pageR = new PageResult<ProductResponseDto>
    {
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int) Math.Ceiling((double) totalCount/ pageSize),
        Data = products.Select(p => new ProductResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Amt = p.Amt,
            CategoryName = p.Category?.Name,
            SaleIds = p.Sales.Select(s => s.Id).ToList()
        }).ToList()



    };

    return TypedResults.Ok(pageR);
});

// GET a specific product by its Id
app.MapGet("/products/{id}", async Task<Results<Ok<ProductResponseDto>, NotFound>> (EcomDb db, int id) =>
{
    var targetProduct = await db.Products
    .Include(p => p.Category)
    .Include(p => p.Sales)
    .FirstOrDefaultAsync(p => p.Id == id);

    if (targetProduct== null) {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(new ProductResponseDto
        {
            Id = targetProduct.Id,
            Name = targetProduct.Name,
            Price = targetProduct.Price,
            Amt = targetProduct.Amt,
            CategoryName = targetProduct.Category?.Name,
            SaleIds = targetProduct.Sales.Select(s => s.Id).ToList()
        });
    }
});

// POST or create a product 
app.MapPost("/products/", async (EcomDb db, Product prod) =>
{
    db.Products.Add(prod);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/Products/{prod.Id}", prod);
});


// PUT or update a product  
app.MapPut("/products/{id}", async Task<Results<Ok<Product>, NotFound>> (EcomDb db, Product newProd, int id) =>
{
    var targetProduct = await db.Products.FindAsync(id);

    if (targetProduct == null) {
        return TypedResults.NotFound();
    } else
    {
        // Update what we are allowed to 
        targetProduct.Amt = newProd.Amt;
        targetProduct.Name = newProd.Name;
        targetProduct.CategoryId = newProd.CategoryId; 


        await db.SaveChangesAsync();

        return TypedResults.Ok(targetProduct);
    }
});

// soft delete a product
app.MapDelete("/products/{id}", async Task<Results<NoContent, NotFound>> (EcomDb db, int id) =>
{
   var targetProd =  await db.Products.FindAsync(id); 
   if (targetProd == null)
    {
        return TypedResults.NotFound();
    } else
    {
        targetProd.IsDeleted = true;
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
});


// GET all sales 
// pagnation capabilites
// supports soft deletes
app.MapGet("/sales/", async (EcomDb db, int page=1, int pageSize=10 ) =>
{
    var sales = await db.Sales 
    .Where(p => !p.IsDeleted) 
    .Include(p => p.Products)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    var totalCount = await db.Sales
    .Where(p => !p.IsDeleted)
    .CountAsync();

    PageResult<SaleResponseDTO> pageR = new PageResult<SaleResponseDTO>
    {
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int) Math.Ceiling((double) totalCount/ pageSize),
        Data = sales.Select(p => new SaleResponseDTO
        {
            Id = p.Id,
            CardType = p.CardType,
            Products = p.Products.Select(x => new SaleProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Amt = x.Amt
            }).ToList()
        }).ToList()

    };

    return TypedResults.Ok(pageR);
}
);

// GET a specific sale by ID
app.MapGet("/sales/{id}", async Task<Results<Ok<SaleResponseDTO>, NotFound>> (EcomDb db, int id) =>
{
    var targetSale = await db.Sales
    .Include(p => p.Products)
    .FirstOrDefaultAsync(p => p.Id == id);

    if (targetSale == null) {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(new SaleResponseDTO
        {
            Id = targetSale.Id,
            CardType = targetSale.CardType,
            Products = targetSale.Products.Select(x => new SaleProductDTO
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Amt = x.Amt
            }).ToList()
        });
    };
});

// POST or create a sale in the table
app.MapPost("/sales/", async (EcomDb db, Sale sale) =>
{
    db.Sales.Add(sale);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/Products/{sale.Id}", sale);
});


// PUT or update a sale
app.MapPut("/sales/{id}", async Task<Results<Ok<Sale>, NotFound>> (EcomDb db, Sale newSale, int id) =>
{
    var targetSale = await db.Sales.FindAsync(id);

    if (targetSale == null) {
        return TypedResults.NotFound();
    } else
    {
        // Update what we are allowed to 
        targetSale.CardType = newSale.CardType;

        await db.SaveChangesAsync();

        return TypedResults.Ok(targetSale);
    }
});

// soft delete a sale
app.MapDelete("/sales/{id}", async Task<Results<NoContent, NotFound>> (EcomDb db, int id) =>
{
   var targetSale =  await db.Sales.FindAsync(id); 
   if (targetSale == null)
    {
        return TypedResults.NotFound();
    } else
    {
        targetSale.IsDeleted = true;
        await db.SaveChangesAsync(); 
        return TypedResults.NoContent();
    }
});

// GET all categories
app.MapGet("/category/", async (EcomDb db, int page=1, int pageSize=10 ) =>
{
    var categories = await db.Categories
    .Include(p => p.Products)
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    var totalCount = await db.Categories
    .CountAsync();

    PageResult<CategoryResponseDTO> pageR = new PageResult<CategoryResponseDTO>
    {
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int) Math.Ceiling((double) totalCount/ pageSize),
        Data = categories.Select(p => new CategoryResponseDTO
        {
            Id = p.Id,
            Name = p.Name,
            ProductNames = p.Products.Select(p => p.Name).ToList()
        }).ToList()

    };

    return TypedResults.Ok(pageR);
}
);

// Get a specific category by Id
app.MapGet("/category/{id}", async Task<Results<Ok<CategoryResponseDTO>, NotFound>> (EcomDb db, int id) =>
{
    var targetCat = await db.Categories
    .Include(p => p.Products)
    .FirstOrDefaultAsync(p => p.Id == id);

    if (targetCat == null) {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(new CategoryResponseDTO
        {
            Id = targetCat.Id,
            Name = targetCat.Name,
            ProductNames = targetCat.Products.Select(p => p.Name).ToList()
        });
    }
});

// POST or create a category
app.MapPost("/category/", async (EcomDb db, Category cat) =>
{
    db.Categories.Add(cat);
    await db.SaveChangesAsync();

    return TypedResults.Created($"/Products/{cat.Id}", cat);
});

// PUT or update a category
app.MapPut("/category/{id}", async Task<Results<Ok<Category>, NotFound>> (EcomDb db, Category newCat, int id) =>
{
    var targetCat = await db.Categories.FindAsync(id);

    if (targetCat == null) {
        return TypedResults.NotFound();
    } else
    {
        // Update what we are allowed to 
        targetCat.Name = newCat.Name;

        await db.SaveChangesAsync();

        return TypedResults.Ok(targetCat);
    }
});

// DELETE a category
app.MapDelete("/category/{id}", async Task<Results<NoContent, NotFound>> (EcomDb db, int id) =>
{
   var targetCat =  await db.Categories.FindAsync(id); 
   if (targetCat == null)
    {
        return TypedResults.NotFound();
    } else
    {
        db.Remove(targetCat);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
});

app.Run();
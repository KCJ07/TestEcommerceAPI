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
    opt.UseSqlite("EcomDb.db");
});

var app = builder.Build();

// GET all products
// pagnation capabilites
// supports soft deletes
app.MapGet("/products/", async (EcomDb db, int page=1, int pageSize=10 ) =>
{
    var products = await db.Products 
    .Where(p => !p.IsDeleted) 
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    var totalCount = await db.Products
    .Where(p => !p.IsDeleted)
    .CountAsync();

    PageResult<Product> pageR = new PageResult<Product>
    {
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int) Math.Ceiling((double) totalCount/ pageSize),
        Data = products

    };

    return TypedResults.Ok();
});

// GET a specific product by its Id
app.MapGet("/products/{id}", async Task<Results<Ok<Product>, NotFound>> (EcomDb db, int id) =>
{
    var targetProduct = await db.Products.FindAsync(id);

    if (targetProduct== null) {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(targetProduct);
    }
});

// POST or create a product 
app.MapPost("/products/", async (EcomDb db, Product prod) =>
{
    db.Products.Add(prod);
    await db.SaveChangesAsync();

    .wh

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
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    var totalCount = await db.Products
    .Where(p => !p.IsDeleted)
    .CountAsync();

    PageResult<Sale> pageR = new PageResult<Sale>
    {
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int) Math.Ceiling((double) totalCount/ pageSize),
        Data = sales

    };

    return TypedResults.Ok();
}
);

// GET a specific sale by ID
app.MapGet("/sales/{id}", async Task<Results<Ok<Sale>, NotFound>> (EcomDb db, int id) =>
{
    var targetSale = await db.Sales.FindAsync(id);

    if (targetSale == null) {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(targetSale);
    }
});

// POST or create a sale in the table
// might not capture many to many correclty>>>?????????????????????????!!!!
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
app.MapGet("/categories/", async (EcomDb db, int page=1, int pageSize=10 ) =>
{
    var categories = await db.Categories
    .Skip((page - 1) * pageSize)
    .Take(pageSize)
    .ToListAsync();

    var totalCount = await db.Categories
    .CountAsync();

    PageResult<Category> pageR = new PageResult<Category>
    {
        TotalCount = totalCount,
        Page = page,
        PageSize = pageSize,
        TotalPages = (int) Math.Ceiling((double) totalCount/ pageSize),
        Data = categories

    };

    return TypedResults.Ok();
}
);

// Get a specific category by Id
app.MapGet("/categories/{id}", async Task<Results<Ok<Category>, NotFound>> (EcomDb db, int id) =>
{
    var targetCat = await db.Categories.FindAsync(id);

    if (targetCat == null) {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(targetCat);
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
app.MapDelete("/categories/{id}", async Task<Results<NoContent, NotFound>> (EcomDb db, int id) =>
{
   var targetSale =  await db.Categories.FindAsync(id); 
   if (targetSale == null)
    {
        return TypedResults.NotFound();
    } else
    {
        db.Remove(id);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
});









































// GET endpoint on parent directory should return list of Shifts 
app.MapGet("/shifts/", async (EcomDb db) => 
    await db.Shifts.ToListAsync());

// GET endpoint gets specific shift object 
app.MapGet("/shifts/{id}", async Task<Results<Ok<Shift>, NotFound>> (int id, Shiftdb db) =>
{
    var targetShift = await db.Shifts.FindAsync(id);

    // tutorial code with newer syntax 
    /*
    return targetShift == null
        ? TypedResults.NotFound()
        : TypedResults.Ok(targetShift);
    */ 
    if (targetShift == null)
    {
        return TypedResults.NotFound();
    } else
    {
        return TypedResults.Ok(targetShift);
    }
});


// POST endpoint should add a shift
app.MapPost("/shifts", async (Shift shift, Shiftdb db) =>
{
    db.Shifts.Add(shift);
    await db.SaveChangesAsync();

    // might need to add $ here 
    return TypedResults.Created("/shifts/{shift.id}", shift);
});


// Delete Endpoint 
app.MapDelete("/shifts/{id}", async Results<NoContent, NotFound> (int id, Shiftdb db) =>
{
    var targetShift = await db.Shifts.FindAsync(id);

    if (targetShift == null)
    {
        return TypedResults.NotFound();
    } else
    {
        db.Shifts.Remove(targetShift);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }
});


app.Run();

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

// GET a specific product by its Id

// POST or create a product 

// PUT or update a product  

// soft delete a product

// GET all sales 

// GET a specific sale by ID

// POST or create a sale in the table

// PUT or update a sale

// soft delete a sale

// GET all categories

// Get a specific category by Id

// POST or create a category

// PUT or update a category

// DELETE a category










































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

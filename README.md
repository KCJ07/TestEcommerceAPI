# Ecommerce API

A RESTful Web API built with ASP.NET Core, Entity Framework Core, and SQLite as part of [The C# Academy](https://thecsharpacademy.com/project/18/ecommerce-api) project series. The API manages a basic retail backend with products, categories, and sales — including a many-to-many relationship between products and sales.

---

## Stack

- .NET 10
- ASP.NET Core Minimal APIs
- Entity Framework Core
- SQLite
- Postman (testing)

---

## Features

- Full CRUD for Products, Categories, and Sales
- Many-to-many relationship between Products and Sales managed via EF join table
- Pagination on GET Products and GET Sales endpoints
- Soft deletes on Products and Sales — records are flagged as deleted rather than removed
- DTO classes on GET endpoints to prevent circular reference errors from EF navigation properties
- SQLite database with EnsureCreated on startup

---

## Endpoints

### Categories
| Method | Route | Description |
|--------|-------|-------------|
| GET | /category/ | Get all categories |
| GET | /category/{id} | Get a specific category |
| POST | /category/ | Create a category |
| PUT | /category/{id} | Update a category |
| DELETE | /category/{id} | Delete a category |

### Products
| Method | Route | Description |
|--------|-------|-------------|
| GET | /products/ | Get all products (paginated) |
| GET | /products/{id} | Get a specific product |
| POST | /products/ | Create a product |
| PUT | /products/{id} | Update a product |
| DELETE | /products/{id} | Soft delete a product |

### Sales
| Method | Route | Description |
|--------|-------|-------------|
| GET | /sales/ | Get all sales (paginated) |
| GET | /sales/{id} | Get a specific sale |
| POST | /sales/ | Create a sale with products |
| PUT | /sales/{id} | Update a sale |
| DELETE | /sales/{id} | Soft delete a sale |

### Pagination

Supported on GET /products/ and GET /sales/ via query parameters:

```
GET /products/?page=1&pageSize=10
GET /sales/?page=1&pageSize=10
```

---

## Running the Project

```bash
git clone https://github.com/KCJ07/TestEcommerceAPI
cd TestEcommerceAPI
dotnet run
```

The database file `EcomDb.db` will be created automatically on first run via `EnsureCreated`.

---

## Testing

A Postman collection and `.http` test file are included in the `Tests/` folder. Run categories first, then products, then sales — each depends on the previous existing in the database.

Recommended order:
```
1. POST /category/     - create categories
2. POST /products/     - create products (requires categoryId)
3. POST /sales/        - create sales (requires productIds)
```

---

## Design Decisions

**Soft deletes on Products and Sales** — in retail, historical records matter. Deleting a product that was part of a past sale would break that record, so products and sales are flagged with `IsDeleted` rather than removed. Categories use a hard delete since no historical data depends on them directly.

**Price is not updatable** — updating a product price after a sale has been made would misrepresent what the customer actually paid. The PUT product endpoint deliberately excludes price from the update.

**DTO classes on responses** — EF navigation properties cause circular reference errors during JSON serialization (Product references Category which references Products which references Category...). Response DTOs flatten the output and break the cycle cleanly.

**Minimal APIs** — chosen over controller-based APIs for simplicity given the scope of the project. All endpoints are defined directly in `Program.cs`.

---

## TODO

- [x] Set up project with ASP.NET Core Minimal APIs
- [x] Configure EF Core with SQLite
- [x] Create Product, Sale, Category models
- [x] Set up many-to-many relationship between Products and Sales
- [x] Implement CRUD endpoints for all three tables
- [x] Add pagination to GET Products and GET Sales
- [x] Add soft deletes to Products and Sales
- [x] Fix circular reference issue with DTO classes on GET endpoints
- [x] Add Postman collection and HTTP test file
- [ ] Add DTO classes to PUT and POST responses to show full relationship data
- [ ] Add validation — required fields, price must be positive, amt must be non-negative
- [ ] Add filtering to GET products — filter by category, price range, availability

---

## Future Work

- Migrate from SQLite to SQL Server for production use
- Add authentication and authorization
- Add an order total field to Sales calculated from product prices at time of sale
- Expand the Category model to support subcategories
- Build a frontend client to consume the API

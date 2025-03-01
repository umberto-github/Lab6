using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add Cors Service
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {   //Client Url
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Add In-Memory Caching Service
builder.Services.AddMemoryCache();

var app = builder.Build();

// Apply the CORS policy
app.UseCors("AllowSpecificOrigin");

// Get the IMemoryCache instance from the DI container
var cache = app.Services.GetRequiredService<IMemoryCache>();

app.MapGet("/api/productlist", (IMemoryCache cache) =>
{
    const string cacheKey = "productList";

    if (!cache.TryGetValue(cacheKey, out var products))
    {
        // The data does not exist in the cache, so fetch it and add it to the cache
        products = new[]
        {
            new
            {
                Id = 1,
                Name = "Laptop",
                Price = 1200.50,
                Stock = 25,
                Category = new { Id = 101, Name = "Electronics" }
            },
            new
            {
                Id = 2,
                Name = "Headphones",
                Price = 50.00,
                Stock = 100,
                Category = new { Id = 102, Name = "Accessories" }
            }
        };

        // Set cache options
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(5));

        // Save the data in the cache
        cache.Set(cacheKey, products, cacheOptions);
    }

    return products;
});

app.Run();

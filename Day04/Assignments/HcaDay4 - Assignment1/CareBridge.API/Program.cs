using CareBridge.EFCoreDemo.Models.Generated;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register EF Core DbContext.
// ASP.NET Core will automatically create and inject it when needed.
builder.Services.AddDbContext<CareBridgeScaffoldContext>();

// Add Swagger support.
// Swagger gives us a testing screen for APIs.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow Vue.js running on another port
// to call this API from the browser.
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Enable Swagger.
app.UseSwagger();
app.UseSwaggerUI();

// Enable CORS.
app.UseCors();

// Simple health-check endpoint.
app.MapGet("/", () =>
{
    return "CareBridge API is running";
});

// Return first 20 patients.
// EF Core converts this LINQ query into SQL.
app.MapGet("/api/patients",
    (
        CareBridgeScaffoldContext db,
        bool? activeOnly
    ) =>
    {
        var query = db.Patients.AsQueryable();

        if (activeOnly == true)
        {
            query = query.Where(p => p.IsActive == true);
        }

        return query
            .Select(p => new
            {
                p.PatientId,
                p.FullName,
                p.City,
                p.IsActive
            })
            .OrderBy(p => p.FullName)
            .Take(20)
            .ToList();
    });

// search by city
app.MapGet("/api/patients/search",
    (CareBridgeScaffoldContext db, string? city, bool? activeOnly) =>
    {
        var query = db.Patients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(city))
        {
            query = query.Where(p => p.City == city);
        }

        if (activeOnly == true)
        {
            query = query.Where(p => p.IsActive == true);
        }

        return query
            .OrderBy(p => p.FullName)
            .Select(p => new
            {
                p.PatientId,
                p.FullName,
                p.City,
                p.IsActive
            })
            .Take(20)
            .ToList();

    });

app.Run();
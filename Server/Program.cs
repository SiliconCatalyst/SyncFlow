using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Server.Components;
using Server.Data;

var builder = WebApplication.CreateBuilder(args);

Env.Load();
string? saPassword = Environment.GetEnvironmentVariable("SA_PASSWORD");
Console.WriteLine($"SA_PASSWORD = {saPassword ?? "NULL"}");

string? connectionString = $"Server=localhost,1433;Database=DataCollectorDB;User Id=sa;Password={saPassword};TrustServerCertificate=True;";

builder.Configuration["ConnectionStrings:DefaultConnection"] = connectionString;

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

// Test endpoint to verify database connection
app.MapGet("/api/test-db", async (ApplicationDbContext db) =>
{
    try
    {
        // Get the underlying connection
        var connection = db.Database.GetDbConnection();

        await connection.OpenAsync(); // <-- will throw if login fails or DB doesn't exist
        await connection.CloseAsync();

        return Results.Ok(new
        {
            connected = true,
            message = "Database connection successful!"
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new
        {
            connected = false,
            message = ex.Message,
            details = ex.ToString()
        });
    }
});

app.Run();

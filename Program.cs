using Microsoft.EntityFrameworkCore;
using MvcDatabaseApp.Data;
using MvcDatabaseApp.Repositories.Contracts;
using MvcDatabaseApp.Repositories.Implementations;
using MvcDatabaseApp.Services;
using MvcDatabaseApp.Services.Contracts;
using MvcDatabaseApp.Services.Implementations;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add DbContext with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//add automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Register Repositories (Scoped lifetime)
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Register Services (Scoped lifetime)
builder.Services.AddScoped<IProductService, ProductService>();

// Add HttpContextAccessor for services that need it
builder.Services.AddHttpContextAccessor();

// Add logging
builder.Services.AddLogging();

builder.Logging.AddFile("logs/app-{Date}.txt");
//Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate(); // Apply migrations and create database
        Console.WriteLine("Database created and seeded successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
    }
}
    app.Run();
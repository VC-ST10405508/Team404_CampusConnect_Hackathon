using Microsoft.EntityFrameworkCore;
using Team404_CampusConnect_Hackathon.Data;
using Team404_CampusConnect_Hackathon.Interface;
using Team404_CampusConnect_Hackathon.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register generic repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserServiceRepository>();
builder.Services.AddScoped<IGroupWithDetails, GroupDetailsRepository>();

// Define database file path (inside Data folder)
var dataFolder = Path.Combine(Directory.GetCurrentDirectory(), "Data");
Directory.CreateDirectory(dataFolder);
var dbPath = Path.Combine(dataFolder, "app.db");

// Configure DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={Path.Combine(dataFolder, "app.db")}"));

// Add session support
builder.Services.AddDistributedMemoryCache(); // In-memory cache for session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Customize session timeout
    options.Cookie.HttpOnly = true; // Make session cookie accessible only to HTTP requests (more secure)
    options.Cookie.IsEssential = true; // Ensure the session cookie is essential for the app to function
});


var app = builder.Build();

//ensuring the tables are created
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

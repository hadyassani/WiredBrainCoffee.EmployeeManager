using Microsoft.EntityFrameworkCore;
using WiredBrainCoffee.EmployeeManager.Data;
using WiredBrainCoffee.EmployeeManager.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContextFactory<EmployeeManagerDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeManagerDb"));
});
builder.Services.AddScoped<StateContainer>();

var app = builder.Build();

// Don't do this in production,just useful for development
await EnsureDatabaseIsMigrated(app.Services);

async Task EnsureDatabaseIsMigrated(IServiceProvider services)
{
    using var scope = services.CreateScope();
    using var ctx = scope.ServiceProvider.GetService<EmployeeManagerDbContext>();
    if(ctx is not null)
    {
       await ctx.Database.MigrateAsync();
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

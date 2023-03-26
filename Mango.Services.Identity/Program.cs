using Mango.Services.Identity;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Mango.Services.Identity.Models.Initializer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
       .AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

 var builderService  =   builder.Services.AddIdentityServer(options =>
                    {
                        options.Events.RaiseErrorEvents= true;
                        options.Events.RaiseInformationEvents= true;
                        options.Events.RaiseFailureEvents= true;
                        options.Events.RaiseSuccessEvents= true;
                        options.EmitStaticAudienceClaim = true;
                    })
                    .AddInMemoryIdentityResources(SD.IdentityResources)
                    .AddInMemoryApiScopes(SD.ApiScopes)
                    .AddInMemoryClients(SD.Clients)
                    .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitilizer>();   

builderService.AddDeveloperSigningCredential();

builder.Services.AddControllersWithViews();

var app = builder.Build();


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
app.UseIdentityServer();

app.UseAuthorization();
SeedDatabase();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}

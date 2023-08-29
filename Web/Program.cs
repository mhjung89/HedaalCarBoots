using Core.Authorization;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services
    .AddDbContext<HCBIdentityDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<HCBUser, IdentityRole>()
    .AddEntityFrameworkStores<HCBIdentityDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = CookieAuthenticationDefaults.CookiePrefix + "HedaalCarBoots";
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

await SeedData(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static async Task SeedData(IServiceCollection services)
{
    using var serviceProvider = services.BuildServiceProvider();

    var userManager = serviceProvider.GetRequiredService<UserManager<HCBUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    bool adminRoleExists = await roleManager.RoleExistsAsync(HCBRoles.Admin);
    if (!adminRoleExists)
    {
        await roleManager.CreateAsync(new IdentityRole(HCBRoles.Admin));
    }


    bool basicRoleExists = await roleManager.RoleExistsAsync(HCBRoles.Basic);
    if (!basicRoleExists)
    {
        await roleManager.CreateAsync(new IdentityRole(HCBRoles.Basic));
    }

    HCBUser adminUser = new HCBUser
    {
        UserName = "admin@hedaal.com",
        Email = "admin@hedaal.com"
    };

    bool adminUserExists = await userManager.FindByEmailAsync(adminUser.Email) != null;

    if (adminUserExists)
    {
        return;
    }

    IdentityResult adminUserCreated = await userManager.CreateAsync(adminUser, "@dmiN1234!");

    if (adminUserCreated.Succeeded)
    {
        await userManager.AddToRoleAsync(adminUser, HCBRoles.Admin);
    }
}
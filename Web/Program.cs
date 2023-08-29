using Core.Authentication;
using Core.Authorization;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HCBDbContext>(options => options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<HCBDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = CookieAuthenticationDefaults.CookiePrefix + "HedaalCarBoots";
});

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services
    .AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy()));
    });

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

    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

    bool adminRoleExists = await roleManager.RoleExistsAsync(HCBRoles.Admin);
    if (!adminRoleExists)
    {
        await roleManager.CreateAsync(new ApplicationRole(HCBRoles.Admin));
    }


    bool basicRoleExists = await roleManager.RoleExistsAsync(HCBRoles.Basic);
    if (!basicRoleExists)
    {
        await roleManager.CreateAsync(new ApplicationRole(HCBRoles.Basic));
    }

    ApplicationUser adminUser = new ApplicationUser
    {
        UserName = "admin@hedaal.com",
        Email = "admin@hedaal.com"
    };

    bool adminUserExists = await userManager.FindByEmailAsync(adminUser.Email) != null;

    if (!adminUserExists)
    {
        IdentityResult adminUserCreated = await userManager.CreateAsync(adminUser, "@dmiN1234!");

        if (adminUserCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, HCBRoles.Admin);
        }
    }

    ApplicationUser basicUser = new ApplicationUser
    {
        UserName = "hedaal@hedaal.com",
        Email = "hedaal@hedaal.com"
    };

    bool basicUserExists = await userManager.FindByEmailAsync(basicUser.Email) != null;

    if (!basicUserExists)
    {
        IdentityResult basicUserCreated = await userManager.CreateAsync(basicUser, "B@sic1234!");

        if (basicUserCreated.Succeeded)
        {
            await userManager.AddToRoleAsync(basicUser, HCBRoles.Basic);
        }
    }
}
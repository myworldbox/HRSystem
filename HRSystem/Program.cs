using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using HRSystem.Infrastructure.Data;
using HRSystem.Infrastructure.Repositories;
using HRSystem.Application.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Clear();
        options.ViewLocationFormats.Add("/Web/Views/{1}/{0}.cshtml");
        options.ViewLocationFormats.Add("/Web/Views/Shared/{0}.cshtml");
    });

builder.Services.AddDbContext<HRSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRSystemContext")));

// Configure Identity with roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
.AddEntityFrameworkStores<HRSystemContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddAutoMapper(config => config.AddProfile<MappingHelper>());

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
app.MapRazorPages();

// Seed roles and users
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    // Create roles
    string[] roleNames = { "Clerical", "Supervisor", "Manager" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Create users
    var users = new[]
    {
        new { Username = "Clerk01", Password = "Password123!", Role = "Clerical" },
        new { Username = "Clerk02", Password = "Password123!", Role = "Clerical" },
        new { Username = "Super01", Password = "Password123!", Role = "Supervisor" },
        new { Username = "Super02", Password = "Password123!", Role = "Supervisor" },
        new { Username = "Mgr01", Password = "Password123!", Role = "Manager" }
    };

    foreach (var user in users)
    {
        if (await userManager.FindByNameAsync(user.Username) == null)
        {
            var identityUser = new IdentityUser { UserName = user.Username, Email = $"{user.Username}@example.com" };
            var result = await userManager.CreateAsync(identityUser, user.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(identityUser, user.Role);
            }
        }
    }
}

app.Run();
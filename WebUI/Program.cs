using BusinessLayer.Abstract;
using BusinessLayer.Concrete;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.DbContext;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RealTimeLayer;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
    opt.Lockout.AllowedForNewUsers = true;
    opt.Lockout.MaxFailedAccessAttempts = 5;
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedEmail = true;
    opt.SignIn.RequireConfirmedPhoneNumber = false;

    opt.Password.RequiredLength = 6;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Cookie ayarları: Login və AccessDenied yönləndirməsi rola görə
builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.LoginPath = "/User/Account/Login";
    opt.AccessDeniedPath = "/User/Account/AccessDenied";
    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    opt.SlidingExpiration = true;

    opt.Events.OnRedirectToLogin = context =>
    {
        var path = context.Request.Path;

        if (path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase))
            context.Response.Redirect("/Admin/Account/Login?ReturnUrl=" + Uri.EscapeDataString(path));
        else
            context.Response.Redirect("/User/Account/Login?ReturnUrl=" + Uri.EscapeDataString(path));

        return Task.CompletedTask;
    };
    opt.Events.OnRedirectToAccessDenied = context =>
    {
        var path = context.Request.Path;

        if (path.StartsWithSegments("/Admin", StringComparison.OrdinalIgnoreCase))
        {
            if (!context.HttpContext.User.IsInRole("admin"))
                context.Response.Redirect("/Admin/Account/AccessDenied");
        }
        else
        {
            if (!context.HttpContext.User.IsInRole("user") && !context.HttpContext.User.IsInRole("admin"))
                context.Response.Redirect("/User/Account/AccessDenied");
        }
        return Task.CompletedTask;
    };

    opt.ExpireTimeSpan = TimeSpan.FromMinutes(10);
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("UserOnly", policy => policy.RequireRole("user", "admin"));
});

builder.Services.AddScoped<IFollowRequestRepository, FollowRequestRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostLikeRepository, PostLikeRepository>();
builder.Services.AddScoped<IPostCommentRepository, PostCommentRepository>();
builder.Services.AddScoped<IUserFollowRepository, UserFollowRepository>();
builder.Services.AddScoped<IUserSocialLinkRepository, UserSocialLinkRepository>();
builder.Services.AddScoped<IUserProfileVisibilityRepository, UserProfileVisibilityRepository>(); 
builder.Services.AddScoped<IUserProfileInfoRepository, UserProfileInfoRepository>();
builder.Services.AddScoped<IUserWorkExperienceRepository, UserWorkExperienceRepository>();
builder.Services.AddScoped<IUserEducationRepository, UserEducationRepository>();

builder.Services.AddScoped<IEmailService, EmailManager>();
builder.Services.AddScoped<IFollowService, FollowManager>();
builder.Services.AddScoped<INotificationService, NotificationManager>();
builder.Services.AddScoped<IPostService, PostManager>();
builder.Services.AddScoped<IUserSocialLinkService, UserSocialLinkManager>();
builder.Services.AddScoped<IUserProfileVisibilityService, UserProfileVisibilityManager>();
builder.Services.AddScoped<IUserProfileInfoService, UserProfileInfoManager>();
builder.Services.AddScoped<IUserWorkExperienceService, UserWorkExperienceManager>();
builder.Services.AddScoped<IUserEducationService, UserEducationManager>();


// Add SignalR
builder.Services.AddSignalR();


var app = builder.Build();

// Map SignalR hubs
app.MapHub<NotificationHub>("/notificationHub");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "Admin",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "User",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    );
    endpoints.MapControllerRoute(
      name: "default",
      pattern: "{controller=Home}/{action=Index}/{id?}"
    );
});
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;



//Create roles
async Task SeedRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "admin", "user" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}
async Task SeedAdminAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    string email = "admin@social.com";
    string password = "Admin123!";

    var admin = await userManager.FindByEmailAsync(email);

    if (admin == null)
    {
        admin = new AppUser
        {
            FirstName = "System",
            LastName = "Administrator",
            UserName = "admin",
            Email = email,
            EmailConfirmed = true
        };

        await userManager.CreateAsync(admin, password);
        await userManager.AddToRoleAsync(admin, "admin");
    }
}
await SeedRolesAsync(app);
await SeedAdminAsync(app);
app.Run();

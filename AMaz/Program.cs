using AMaz.DB;
using AMaz.Entity;
using AMaz.Repo;
using AMaz.Service;
using AMaz.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using AMaz.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AMazDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AMazContext"));
});

builder.Services.AddDefaultIdentity<User>(option =>
{
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedAccount = false;
    option.SignIn.RequireConfirmedEmail = false;

}).AddRoles<IdentityRole>().AddEntityFrameworkStores<AMazDbContext>();

builder.Services.ConfigureApplicationCookie(option =>
{
    option.AccessDeniedPath = "/AccessDenied";
    option.LoginPath = "/Login";
    option.LogoutPath = "/Logout";

});

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<AMazDbContext>();

//AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    //Add Mapping profile here
    mc.AddProfile<FileProfile>();
    mc.AddProfile<AdminProfile>();
    mc.AddProfile<LoginProfile>();
    mc.AddProfile<UserProfile>();
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.Configure<LocalFileStorageConfiguration>(builder.Configuration.GetSection("LocalFileStorageConfiguration"));
builder.Services.Configure<PowerUserConfiguration>(builder.Configuration.GetSection("PowerUserConfiguration"));
#region Biz Services
//builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddTransient<ILoginService, LoginService>();

builder.Services.AddTransient<IFileRepository, FileRepository>();
builder.Services.AddTransient<FileService>();

builder.Services.AddTransient<UserService>();
#endregion

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

app.UseAuthentication();
app.UseAuthorization();

DbInitializer.InitializeAsync(app);


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

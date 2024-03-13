using AMaz.DB;
using AMaz.Entity;
using AMaz.Repo;
using AMaz.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AMazDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("AMazContext"));
});
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAdminResponsitory,AdminReponsitory>();

//AutoMapper
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.CreateMap<User, AuthenticateResponse>().ReverseMap();
    mc.CreateMap<CreateRequest, User>();
    //Add Mapping profile here
});
IMapper mapper = mapperConfig.CreateMapper();

builder.Services.AddSingleton(mapper);

builder.Services.AddTransient<ILoginService, LoginService>();
builder.Services.AddTransient<ILoginRepository, LoginRepository>();

builder.Services.AddHttpContextAccessor();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


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

app.UseAuthorization();
DbInitializer.Initialize(app);

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

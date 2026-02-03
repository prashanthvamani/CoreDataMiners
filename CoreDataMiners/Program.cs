using DataminersBAL;
using DataminersDAL.DBConncetion;
using DataminersDAL.Repositories;
using DataMinersWeb.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSession(
    options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(5);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential= true;
    });

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilter>();

});

builder.Services.AddSingleton<DatabaseHelper>();
builder.Services.AddSingleton<LoginRepository>();
builder.Services.AddScoped<LoginBaL>();
builder.Services.AddSingleton<UserCredentialsRepository>();
builder.Services.AddScoped<RequestFormRepository>();
builder.Services.AddScoped<CustomExceptionFilter>();
builder.Services.AddScoped<ErrorLogRepository>();
builder.Services.AddScoped<ITDBCheckerRepository>();
builder.Services.AddScoped<ITDBLoginRepository>();


var app = builder.Build();

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

app.UseAuthorization();
app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RequestorLogin}/{action=Login}/{id?}");
//pattern: "{controller=ITDBCheckerDetails}/{action=Index}/{id?}");

app.MapControllers();
    
app.Run();

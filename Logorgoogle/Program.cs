using Microsoft.EntityFrameworkCore;
using Logorgoogle.Data;
using Logorgoogle.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
}).AddFacebook(options =>
{
    options.AppId = configuration["Authentication:Facebook:AppId"];
    options.AppSecret = configuration["Authentication:Facebook:AppSecret"];
});
builder.Services.AddDbContext<LogorgoogleContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("LogorgoogleContextConnection")));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<LogorgoogleContext>(options =>
//    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<LogorgoogleUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<LogorgoogleContext>();
//builder.Services.AddDefaultIdentity<LogorgoogleUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<LogorgoogleContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
 
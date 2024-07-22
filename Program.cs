using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Zhankui_Wang_ProblemAssignment2.Data;
using Zhankui_Wang_ProblemAssignment2.Services;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("Zhankui_Wang_ProblemAssignment2ContextConnection") ?? throw new InvalidOperationException("Connection string 'Zhankui_Wang_ProblemAssignment2ContextConnection' not found.");

builder.Services.AddDbContext<Zhankui_Wang_ProblemAssignment2Context>(options => options.UseSqlServer(connectionString));

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HandleCookies>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<Zhankui_Wang_ProblemAssignment2Context>();

// Add services to the container.
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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();        //super important!, or can't parse asp- tag, razor pages
app.Run();

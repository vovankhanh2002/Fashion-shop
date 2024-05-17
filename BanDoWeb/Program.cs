using BanDoWeb.Access.Dbcontext;
using Microsoft.EntityFrameworkCore;
using Project.DataAccess.Repository;
using Project.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using BanDoWeb.Model.Models;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using NToastNotify;
using BanDoWeb.Areas.Hubs;
using ServiceStack;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Configuration;
using CodeMegaVNPay.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.BottomRight; });
builder.Services.AddRazorPages().AddNToastNotifyNoty(new NotyOptions
{
    ProgressBar = true,
    Timeout = 5000
});

builder.Services.AddDbContext<DbcontextBanDo>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DbContext"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DbcontextBanDo>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<TwilioSetting>(builder.Configuration.GetSection("Twilio"));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.SignIn.RequireConfirmedEmail = true;
});
builder.Services.AddAuthentication()
    .AddGoogle(googleOptions =>
    {
        IConfigurationSection googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
        googleOptions.ClientId = googleAuthNSection["ClientId"];
        googleOptions.ClientSecret = googleAuthNSection["ClientSecret"];
    })
    .AddFacebook(options =>
    {
        IConfigurationSection facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
        options.AppId = facebookAuthNSection["AppId"];
        options.AppSecret = facebookAuthNSection["AppSecret"];
    });
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
app.UseCookiePolicy();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapHub<SignalsServer>("/signalsServer");
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chatHub", option =>
    {
        option.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
    });
});
app.UseNToastNotify();
app.UseNotyf();
app.MapRazorPages();
app.MapControllerRoute(
    name: "Area",
    pattern: "{area:exists}/{controller=HomeAdmin}/{action=Index}/{id?}");

    
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

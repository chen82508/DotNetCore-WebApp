using HR.Areas.Frontend.Service;
using HR.Areas.HR.Service;
using HR.Database;
using HR.DTOs.Frontend;
using HR.DTOs.HR;
using HR.Interface;
using HR.Service;
using HR.Utils.Authentication.Login;
using HR.Utils.Crypto;
using HR.Utils.Export;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton(provider => builder.Configuration);

builder.Services.AddDbContext<HRSysContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRsys"));
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddTransient<IExport, ExportService>();

builder.Services.AddTransient<IAuthenticate<IFormCollection>, LoginAuthentication>();
builder.Services.AddTransient<ICrypto, PasswordCrypto>();
builder.Services.AddTransient<IEntityService<Option>, OptionService>();
builder.Services.AddTransient<IEntityService<OptionTag>, OptionTagService>();
builder.Services.AddTransient<IEntityService<JobType>, JobTypeService>();
builder.Services.AddTransient<IEntityService<JobTypeTag>, JobTypeTagService>();
builder.Services.AddTransient<IEntityService<Message>, MessageService>();
builder.Services.AddTransient<IEntityService<MessageTag>, MessageTagService>();
builder.Services.AddTransient<IEntityService<JobInfo>, JobInfoService>();
builder.Services.AddTransient<IEntityService<HumResListInfo>, HumResListInfoService>();
builder.Services.AddTransient<IEntityService<HumResEditInfo>, HumResEditInfoService>();
builder.Services.AddTransient<IEntityService<StatisticData>, StatisticDataService>();
builder.Services.AddTransient<IEntityService<UserInfo>, UserService>();

builder.Services.AddTransient<IEntityService<ResumeForm>, ResumeFormService>();
builder.Services.AddTransient<IEntityService<ResumeAttachment>, ResumeAttachmentService>();
builder.Services.AddTransient<IEntityService<ConsistencyTest>, ConsistencyTestService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddOptions<LoginAuthOptions>();
builder.Services.AddOptions<CookieAuthenticationOptions>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "userauth";
        options.LoginPath = "/Login";
    });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error500");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseExceptionHandler("/Error/Error500");
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("~/Error404.html");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller}/{action}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();

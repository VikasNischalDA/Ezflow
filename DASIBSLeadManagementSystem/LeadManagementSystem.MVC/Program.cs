using LeadManagementSystem.Model.Models;
using LeadManagementSystem.MVC.Services;
using LeadManagementSystem.MVC.Services.Implementations;
using LeadManagementSystem.MVC.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddCors();
builder.Services.AddDistributedMemoryCache(); //Required for session storage
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpClient(configuration.GetSection("ApiHttpClient").Value, httpClient =>
{
    httpClient.BaseAddress = new Uri(configuration.GetSection("ApiBaseUrl").Value);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    return new HttpClientHandler();
});



builder.Services.AddTransient<IBaseHttpClientService, BaseHttpClientService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IDashboardService, DashboardService>();
builder.Services.AddTransient<ILeadSourceService, LeadSourceService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IQueueManagementService, QueueManagementService>();
builder.Services.Configure<ApiConfig>(options =>
{
    options.ApiBaseUrl = builder.Configuration.GetSection("ApiBaseUrl").Value;
    options.ApiHttpClient = builder.Configuration.GetSection("ApiHttpClient").Value;
});
builder.Services.AddControllersWithViews()
        .AddMvcOptions(options => options.EnableEndpointRouting = false)
        .AddViewOptions(options =>
        {
            options.HtmlHelperOptions.ClientValidationEnabled = true;
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
app.UseSession();
app.UseStaticFiles();
app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=QueueManagement}/{action=Index}");

app.Run();

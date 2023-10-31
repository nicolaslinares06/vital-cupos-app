using CUPOS_FRONT.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

// Obtiene referencia a appsettings.json

IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

var key = configuration.GetValue<string>("JWTSettings:securitykey");

//builder.Services.AddScoped(hc => new HttpClient { BaseAddress = new Uri("https://localhost:7053/") });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Agregar las sesiones 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(40);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(option =>
{
    option.LoginPath = "/Login/Index";
    option.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    option.AccessDeniedPath = "/Home/Privacy";
});

builder.Services.AddTransient<IPDFMetodos, PDFMetodos>();
builder.Services.AddTransient<IFiltrosDeBusqueda, FiltrosDeBusqueda>();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}"
);

app.UseSession();
app.Run();

using ECommerce.DataAccess;
using ECommerce.DataAccess.Repository;
using ECommerce.DataAccess.Repository.IRepository;
using ECommerce.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Agregar soporte para EmailSender
builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Configuracion contexto de la cadena de conexion
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Soporte para la configuracion de Stripe
builder.Services.Configure<ConfiguracionStripe>(builder.Configuration.GetSection("Stripe"));

// Configuracion de Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(opciones =>
    // Establecer si se requiere confirmacion de cuenta para iniciar sesion  
    opciones.SignIn.RequireConfirmedAccount = false
).AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Soporte para Cookies de autenticacion y autorizacion
builder.Services.ConfigureApplicationCookie(options => 
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
    options.Cookie.HttpOnly = true; // Mitigar ataques XSS
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Tiempo de expiracion de la cookie
    options.SlidingExpiration = true; // Renovar cookie con cada solicitud
});

builder.Services.AddDistributedMemoryCache(); // Soporte para cache en memoria

// Soporte para trabajo con sesiones
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de inactividad antes de expirar la sesion
    options.Cookie.HttpOnly = true; // Mitigar ataques XSS
    options.Cookie.IsEssential = true; // Asegurar que la cookie se cree aunque el usuario no acepte cookies
});

// Agregar repositorios al contenedor de inyeccion de dependencias
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Configurar la clave secreta de Stripe
string key = builder.Configuration.GetSection("Stripe:ClaveSecreta").Get<string>();
StripeConfiguration.ApiKey = key;

// Autenticación debe ir antes de autorización
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Redirigir manualmente a la pagina deseada
app.MapGet("/", context =>
{
    context.Response.Redirect("/Cliente/Inicio/Index");
    return Task.CompletedTask;
});

app.Run();

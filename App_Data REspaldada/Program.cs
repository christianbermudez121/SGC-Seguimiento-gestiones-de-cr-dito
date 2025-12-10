using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProyectoSGCBLL.Services;
using ProyectoSGCDAL.Data;
using ProyectoSGCDAL.Entities;
using ProyectoSGCDAL.Repositories;

var builder = WebApplication.CreateBuilder(args);

// MVC
var mvc = builder.Services.AddControllersWithViews();
#if DEBUG
mvc.AddRazorRuntimeCompilation();
#endif

// ★ Construir ruta ABSOLUTA y asegurar carpeta
var dataDir = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(dataDir);
var dbPath = Path.Combine(dataDir, "sgc.db");

// DB Sqlite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite($"Data Source={dbPath};Cache=Shared"));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/Login";
});

// Dependency Injection
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// --- AHORA SÍ: construir la app ---
var app = builder.Build();

// APLICAR MIGRACIONES Y SEEDER
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // ← CREA TABLAS DE IDENTITY SI NO EXISTEN
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();

    var userMgr = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleMgr = services.GetRequiredService<RoleManager<IdentityRole>>();

    await IdentitySeeder.SeedAsync(userMgr, roleMgr);
}

// Middlewares
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

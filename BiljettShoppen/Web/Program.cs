using Application;
using DataAccess;
using DataAccess.Utils;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Application.Hubs;

// TODO: Få klart skapande av ny SeatLayout (camilla håller på).
// TODO: Skapa sida för att skapa SeatLayout.
// TODO: Medelande när betalning lyckas.
// TODO: Bättre felmedelanden.
// TODO: Fortsätt på beräkna pris decorator, ska beräkna pris på tidpunkt (alex håller på).
// TODO: Sida för att se uppkommande evenemang (handler finns). 
// TODO: Möjlighet att ladda ner boknings PDF.
// TODO: Söka på bokningsnummer (tom håller på).
// TODO: Lägg till stubbar för kort och faktura betalning.
// TODO: Email stubb.
// TODO: Skapa egna specifika exceptions.
// TODO: Spara faktura/bokings (pdfer) i databasen.
// TODO: Skapa faktura i databasen (pdf?).
// TODO: Ta bort bokning från timern om användaren lämnar betalningssidan.

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDataAccessLayer(builder.Configuration);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddApplicationLayer();

builder.Services.AddControllersWithViews()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

    // Generera fake data under utveckling om databasen är tom.
    using var scope = app.Services.CreateScope();
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.SeedAsync();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.MapHub<BookingHub>("/bookinghub");

app.Run();

using Application;
using Application.BackgroundServices;
using Application.Features.Payments.TransactionConfirmation;
using Application.Interfaces;
using DataAccess;
using DataAccess.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models.Entities;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Serialization;

// TODO: Skapa entreer
// TODO: När en Arena skapas ska en Huvudentree automatiskt skapas.
// TODO: Få klart skapande av ny SeatLayout.
// TODO: Skapa sida för att skapa SeatLayout.
// TODO: Medelande när 10 minuters bokningstimer är slut.
// TODO: Medelande när betalning lyckas.
// TODO: Bättre felmedelanden.
// TODO: Fortsätt på beräkna pris decorator, ska beräkna pris på evenamngstyp och tidpunkt.
// TODO: Sida för att se uppkommande evenemang. 
// TODO: Optimera BookingTimer.
// TODO: Möjlighet att ladda ner boknings PDF.
// TODO: Söka på bokningsnummer.
// TODO: Lägg till stubbar för kort och faktura betalning.
// TODO: Email stubb.
// TODO: Skapa egna specifika exceptions.

// TODO: Prata med Hans x2 angående loger på hemsidan, desginen finns för det men inte på hemsidan?. 

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

app.Run();

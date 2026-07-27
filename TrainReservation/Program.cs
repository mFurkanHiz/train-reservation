using System.Diagnostics;
using TrainReservation.Data;
using TrainReservation.Entities;
using TrainReservation.Models.ViewModel;
using TrainReservation.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

/****************************************************************/

builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("TR_Database")));

builder.Services.AddScoped<Repos<Train>>();
builder.Services.AddScoped<Repos<Wagon>>();
builder.Services.AddScoped<ReservationDetail>();
builder.Services.AddScoped<ReservationRequest>();
builder.Services.AddScoped<ReservationResponse>();
builder.Services.AddScoped<TrainRequest>();
builder.Services.AddScoped<WagonRequest>();

/****************************************************************/

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

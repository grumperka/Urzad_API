using KSiwiak_Urzad_API.Data;
using KSiwiak_Urzad_API.Models;
using Microsoft.EntityFrameworkCore;
using Urzad_KSiwiak.Models;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IBackgroundQueue<Akty_slubow>, BackgroundQueue<Akty_slubow>>();
//builder.Services.AddSingleton<IBackgroundQueue<Akty_rozwodu>, BackgroundQueue<Akty_rozwodu>>();
//builder.Services.AddSingleton<IBackgroundQueue<Akty_urodzenia>, BackgroundQueue<Akty_urodzenia>>();
//builder.Services.AddSingleton<IBackgroundQueue<Akty_zgonu>, BackgroundQueue<Akty_zgonu>>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod()
                                                .AllowCredentials();
                      });

//    options.AddPolicy("AllowAll", p =>
//    {
//        p.AllowAnyOrigin()
//        .AllowAnyHeader()
//        .AllowAnyMethod()
//        .AllowCredentials()
//        .SetIsOriginAllowed(hostName => true);
//});
});

builder.Services.AddDbContext<UrzadDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UrzedyDBConnectionString")));
builder.Services.AddScoped<IUrzadDBContextProvider, UrzadDBContextProvider>();

builder.Services.AddDistributedMemoryCache();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(4);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();

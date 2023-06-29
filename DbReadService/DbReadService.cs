using EventDrivenAppDemo.Core.Models;
using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

////var connectionString = builder.Configuration.GetConnectionString("AppDb");
////builder.Services.AddTransient<DataSeeder>();
//////Add Repository Pattern
////builder.Services.AddScoped<IDataRepository, DataRepository>();
////builder.Services.AddDbContext<SensorDataDbContext>(x => x.UseSqlServer(connectionString));
//var app = builder.Build();
//app.Run();

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddTransient<DataSeeder>();
//Add Repository Pattern
builder.Services.AddScoped<IDataRepository, DataRepository>();
builder.Services.AddDbContext<SensorDataDbContext>(x => x.UseSqlServer(connectionString));
var app = builder.Build();

app.MapGet("/", () => "DB Read Service!");

app.Run();

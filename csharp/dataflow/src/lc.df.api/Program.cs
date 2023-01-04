using lc.df.api.Jobs;
using lc.df.api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddHostedService<SimpleDataJob>()
    .AddScoped<ISimpleDataService, SimpleDataService>()
    .AddSingleton<IDataService, DataService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UsePathBase("/api");
app.MapControllers();

app.Run();
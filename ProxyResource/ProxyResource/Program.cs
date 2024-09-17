using ProxyResource.Interfaces;
using ProxyResource.Services;
using Serilog;
var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("../logs/log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddHttpClient();
builder.Services.AddScoped<IResourceService, ResourceService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
Log.Information("Starting web host");
app.Run();

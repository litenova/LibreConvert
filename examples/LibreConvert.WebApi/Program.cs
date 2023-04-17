using LibreConvert.WebApi;
using Microsoft.OpenApi.Models;

var apiName = $"LibreConvert {RunningOperatingSystem.Name} API";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = apiName,
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", apiName);
    c.RoutePrefix = string.Empty; // Set Swagger UI at apps root
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
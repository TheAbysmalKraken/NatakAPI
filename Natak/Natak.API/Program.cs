using Natak.API;
using Natak.API.Logging;
using Natak.API.RateLimiting;
using Natak.Core;
using Natak.Infrastructure;
using Natak.Infrastructure.Middleware;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCore();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddRateLimiting();
builder.Services.AddLogger();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5218")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyAllowSpecificOrigins);

app.UseRateLimiter();

app.UseAuthorization();

app.UseMiddleware<ApiExceptionHandlingMiddleware>();

app.MapControllers();

app.MapEndpoints();

app.Run();

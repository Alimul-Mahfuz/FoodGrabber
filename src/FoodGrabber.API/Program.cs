using FoodGrabber.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationModules(builder.Configuration);
builder.Services.AddSwagger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodGrabber API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at the root
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
await app.Services.SeedIdentityAsync(builder.Configuration);

app.Run();


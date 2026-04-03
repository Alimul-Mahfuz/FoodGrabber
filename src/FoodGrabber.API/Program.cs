using FoodGrabber.API.Extensions;
using FoodGrabber.Inventory.Extensions;
using FoodGrabber.Identity.Extensions;
using FoodGrabber.Menu.Extensions;
using FoodGrabber.Order.Extensions;
using FoodGrabber.Product.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAntiforgery();
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

app.UseFrontendCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapAuthEndpoints();
app.MapUserEndPoint();
app.MapMenuEndpoints();
app.MapProductEndpoints();
app.MapInventoryEndpoints();
app.MapOrderEndpoint();
await app.Services.SeedIdentityAsync(builder.Configuration);
await app.Services.SeedProductModuleAsync();

app.Run();


using Microsoft.Extensions.DependencyInjection;

namespace FoodGrabber.Restaurant.Extensions;

public static class RestaurantModuleExtensions
{
    public static IServiceCollection AddRestaurantModule(this IServiceCollection services)
    {
        return services;
    }
}

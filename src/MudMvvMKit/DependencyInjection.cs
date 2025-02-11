using BlazorTrivialJs;
using Microsoft.Extensions.DependencyInjection;
using MudMvvMKit.Base;

namespace MudMvvMKit;

public static class DependencyInjection
{
    public static void AddMudComposite(this IServiceCollection services)
    {
        services.AddScoped<MudUtility>();
        services.AddScoped<ITrivialJs, TrivialJs>();
    }
}
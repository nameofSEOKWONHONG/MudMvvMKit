using BlazorTrivialJs;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using MudComposite.Base;
using MudCompositeApp;
using MudCompositeApp.Composites;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IWeatherListViewComposite, WeatherDataGridComposite>();
builder.Services.AddScoped<IWeatherDetailComposite, WeatherDetailComposite>();
builder.Services.AddScoped<MudUtility>();
builder.Services.AddTrivialJs();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
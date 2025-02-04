using eXtensionSharp;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudComposite;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.DetailView;

namespace MudCompositeApp.Composites;

public interface IWeatherDetailComposite : IMudDetailViewModel<WeatherForecast>
{
    void Initialize();
}

public class WeatherDetailComposite : MudDetailViewModel<WeatherForecast>, IWeatherDetailComposite
{
    private readonly IWeatherService _weatherService;
    public WeatherDetailComposite(MudViewModelItem mudViewModelItem,
        IWeatherService weatherService) : base(mudViewModelItem)
    {
        _weatherService = weatherService;
    }

    public void Initialize()
    {
        this.OnRetrieve = async () => await _weatherService.Get(this.SelectedItem.Id);
        this.OnSubmit = async () =>
        {
            if (this.SelectedItem.xIsEmpty())
            {
                return await _weatherService.Add(this.SelectedItem);    
            }
            
            return await _weatherService.Modify(this.SelectedItem);
        };
    }
}
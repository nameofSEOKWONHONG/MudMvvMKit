using eXtensionSharp;
using FluentValidation;
using MudMvvMKit.ViewComponents.ViewModels.DetailView;

namespace MudMvvMKitApp.ViewModels;

public interface IWeatherDetailViewModel : IMudDetailViewModel<int, WeatherForecast>
{
    void Initialize();
}

public class WeatherDetailViewModel : MudDetailViewModel<int, WeatherForecast>, IWeatherDetailViewModel
{
    private readonly IWeatherService _weatherService;
    public WeatherDetailViewModel(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public void Initialize()
    {
        this.OnRetrieve = async () =>
        {
            var result = await _weatherService.Get(this.Parameter);
            this.RetrievedItem = result.Data;
            return result;
        };
        this.OnSubmit = async () =>
        {
            if (this.RetrievedItem.Id <= 0)
            {
                return await _weatherService.Add(this.RetrievedItem);    
            }
            
            return await _weatherService.Modify(this.RetrievedItem);
        };
    }
}

public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
{
    public WeatherForecastValidator()
    {
        this.RuleFor(x => x.TemperatureC).GreaterThan(6);
    }
        
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var name = propertyName.xSplit(".")[1];        
        var result = await ValidateAsync(ValidationContext<WeatherForecast>.CreateWithOptions((WeatherForecast)model, x => x.IncludeProperties(name)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };        
}
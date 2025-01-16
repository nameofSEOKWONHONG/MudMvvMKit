using MudBlazor;
using MudComposite;
using MudComposite.Composites.DetailView;

namespace MudCompositeApp.Composites;

public interface IWeatherDetailComposite : IMudDetailViewComposite<WeatherForecast>
{
    void Initialize();
}

public class WeatherDetailComposite : MudDetailViewComposite<WeatherForecast>, IWeatherDetailComposite
{
    private readonly IWeatherService _weatherService;
    public WeatherDetailComposite(IDialogService dialogService, ISnackbar snackbar, IWeatherService weatherService) : base(dialogService, snackbar)
    {
        _weatherService = weatherService;
    }

    public void Initialize()
    {
        this.OnRetrieve = async () => await _weatherService.Get(this.RetrieveItem.Id);
        this.OnSubmit = async () =>
        {   
            if (this.RetrieveItem.Id > 0)
            {
                return await _weatherService.Modify(this.RetrieveItem);
            }

            return await _weatherService.Add(this.RetrieveItem);
        };
    }
}
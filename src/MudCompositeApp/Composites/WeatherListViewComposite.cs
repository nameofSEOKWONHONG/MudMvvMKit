using MudBlazor;
using MudComposite;

namespace MudCompositeApp.Composites;

public class Request
{
    
}

public class SearchModel
{
    
}

public interface IWeatherListViewComposite : IMudListViewComposite<WeatherForecast, SearchModel>
{
    
}

public class WeatherListViewComposite : MudListViewComposite<WeatherForecast, SearchModel>, IWeatherListViewComposite
{
    public WeatherListViewComposite(IDialogService dialogService, ISnackbar snackbar) : base(dialogService, snackbar)
    {
    }
}
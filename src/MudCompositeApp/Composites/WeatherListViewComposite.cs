using MudBlazor;
using MudComposite;

namespace MudCompositeApp.Composites;

public class SearchModel
{
    public string City { get; set; }
}

public interface IWeatherListViewComposite : IMudListViewComposite<WeatherForecast, SearchModel>
{
    
}

public class WeatherListViewComposite : MudListViewComposite<WeatherForecast, SearchModel>, IWeatherListViewComposite
{
    public WeatherListViewComposite(IDialogService dialogService, ISnackbar snackbar) : base(dialogService, snackbar)
    {
    }

    public override string RowStyleFunc(WeatherForecast item, int id)
    {
        return this.SelectedItem == item ? "background-color: #cff4ff;": string.Empty;
    }
}
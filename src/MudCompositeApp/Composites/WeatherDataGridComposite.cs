using MudBlazor;
using MudComposite;
using MudComposite.Composites.ListView;

namespace MudCompositeApp.Composites;

public class SearchModel
{
    public string City { get; set; }
}

public interface IWeatherListViewComposite : IMudDataGridComposite<WeatherForecast, SearchModel>
{
    
}

public class WeatherDataGridComposite : MudDataGridComposite<WeatherForecast, SearchModel>, IWeatherListViewComposite
{
    public WeatherDataGridComposite(IDialogService dialogService, ISnackbar snackbar) : base(dialogService, snackbar)
    {
    }

    public override string RowStyleFunc(WeatherForecast item, int id)
    {
        return this.SelectedItem == item ? "background-color: #cff4ff;": string.Empty;
    }
}
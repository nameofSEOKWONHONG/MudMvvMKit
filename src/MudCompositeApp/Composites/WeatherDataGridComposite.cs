using Microsoft.AspNetCore.Components;
using MudBlazor;
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
    private readonly IWeatherService _weatherService;
    public WeatherDataGridComposite(IDialogService dialogService, 
        ISnackbar snackbar, 
        NavigationManager navigationManager,
        IWeatherService weatherService) : base(dialogService, snackbar, navigationManager)
    {
        _weatherService = weatherService;
    }

    public override void Initialize()
    {
        this.OnServerReload = async (state) =>
        {
            var result = await _weatherService.GetList(this.SearchModel, state.Page, state.PageSize);
            return new GridData<WeatherForecast>()
            {
                TotalItems = result.TotalCount,
                Items = result.Datum
            };
        };
        this.OnRemove = async (item) => await _weatherService.Remove(this.SelectedItem.Id);
    }


    public override string RowStyleFunc(WeatherForecast item, int id)
    {
        return this.SelectedItem == item ? "background-color: #cff4ff;": string.Empty;
    }

    public override void GoDetail()
    {
        this.NavManager.NavigateTo($"/weather/detail/{this.SelectedItem.Id}");
    }
}
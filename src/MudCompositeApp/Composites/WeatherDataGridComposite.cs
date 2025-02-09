using eXtensionSharp;
using MudBlazor;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.ListView;

namespace MudCompositeApp.Composites;

public class SearchModel
{
    public string City { get; set; }
}

public interface IWeatherListViewComposite : IMudDataGridViewModel<WeatherForecast, SearchModel>
{
    
}

public class WeatherDataGridComposite : MudDataGridViewModel<WeatherForecast, SearchModel>, IWeatherListViewComposite
{
    private readonly IWeatherService _weatherService;
    public WeatherDataGridComposite(MudViewModelItem mudViewModelItem,
        IWeatherService weatherService) : base(mudViewModelItem)
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
        this.OnClick = (key, item) =>
        {
            var selectedItem = item.xAs<WeatherForecast>();

            if (key == "detail")
            {
                this.Utility.NavigationManager.NavigateTo($"/weather/detail/{selectedItem.Id}");
            }

            return Task.CompletedTask;
        };
    }


    public override string RowStyleFunc(WeatherForecast item, int id)
    {
        return this.SelectedItem == item ? "background-color: #cff4ff;": string.Empty;
    }

    public override void GoDetail()
    {
        this.Utility.NavigationManager.NavigateTo($"/weather/detail/{this.SelectedItem.Id}");
    }
}
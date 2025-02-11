using eXtensionSharp;
using MudBlazor;
using MudMvvMKit.Base;
using MudMvvMKit.ViewComponents.ViewModels.ListView;

namespace MudMvvMKitApp.ViewModels;

public class SearchModel
{
    public string City { get; set; }
}

public interface IWeatherListViewComposite : IMudListViewModel<WeatherForecast, SearchModel>
{
    
}

public class WeatherListViewModel : MudListViewModel<WeatherForecast, SearchModel>, IWeatherListViewComposite
{
    private readonly IWeatherService _weatherService;

    public WeatherListViewModel(IWeatherService weatherService, MudUtility mudViewUtility)
    : base(mudViewUtility)
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
                this.MudUtility.NavigationManager.NavigateTo($"/weather/detail/{selectedItem.Id}");
            }

            return Task.CompletedTask;
        };
    }
}
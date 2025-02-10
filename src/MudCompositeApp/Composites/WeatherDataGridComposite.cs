using eXtensionSharp;
using Microsoft.AspNetCore.Components;
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

    public WeatherDataGridComposite(IWeatherService weatherService, MudUtility mudViewUtility)
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
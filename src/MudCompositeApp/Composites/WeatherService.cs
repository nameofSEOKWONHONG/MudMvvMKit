using System.Net.Http.Json;
using MudComposite;

namespace MudCompositeApp.Composites;

public interface IWeatherService
{
    Task<PaginatedResult<WeatherForecast>> GetList(SearchModel searchModel, int pageNo = 0, int pageSize = 10);
}

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginatedResult<WeatherForecast>> GetList(SearchModel searchModel, int pageNo = 0, int pageSize = 10)
    {
        var result = await _httpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
        var list = new List<WeatherForecast>();
        list.AddRange(result);
        list.AddRange(result);
        list.AddRange(result);
        list.AddRange(result);
        list.AddRange(result);
        var where = list.Skip(pageNo * pageSize).Take(pageSize).ToList();
        return await PaginatedResult<WeatherForecast>.SuccessAsync(where, list.Count, pageNo, pageSize);
    }
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
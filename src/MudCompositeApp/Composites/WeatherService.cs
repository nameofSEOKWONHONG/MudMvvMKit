using System.Net.Http.Json;
using eXtensionSharp;
using MudComposite;

namespace MudCompositeApp.Composites;

public interface IWeatherService
{
    Task<PaginatedResult<WeatherForecast>> GetList(SearchModel searchModel, int pageNo = 0, int pageSize = 10);
    Task<Results<bool>> Remove(int id);
    Task<Results<WeatherForecast>> Get(int id);
    Task<Results<bool>> Modify(WeatherForecast item);
    Task<Results<int>> Add(WeatherForecast item);
}

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly List<WeatherForecast> _forecasts;
    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _forecasts = new List<WeatherForecast>();
    }

    public async Task<PaginatedResult<WeatherForecast>> GetList(SearchModel searchModel, int pageNo = 0, int pageSize = 10)
    {
        if (_forecasts.xIsEmpty())
        {
            var result = await _httpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
            _forecasts.AddRange(result);
        }
        var items = _forecasts.Skip(pageNo * pageSize).Take(pageSize).ToList();
        return await PaginatedResult<WeatherForecast>.SuccessAsync(items, _forecasts.Count, pageNo, pageSize);
    }

    public async Task<Results<WeatherForecast>> Get(int id)
    {
        //var result = await _httpClient.GetFromJsonAsync<Results<WeatherForecast>>($"api/weather/{id}");
        var result = await _httpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
        var item = result.FirstOrDefault(m => m.Id == id);
        return await Results<WeatherForecast>.SuccessAsync(item);
    }

    public async Task<Results<bool>> Modify(WeatherForecast item)
    {
        // var res = await _httpClient.PatchAsJsonAsync($"api/weather", item);
        // res.EnsureSuccessStatusCode();
        // return await res.Content.ReadFromJsonAsync<Results<bool>>();
        return await Results<bool>.SuccessAsync(true);
    }

    public async Task<Results<int>> Add(WeatherForecast item)
    {
        // var res = await _httpClient.PostAsJsonAsync($"api/weather", item);
        // res.EnsureSuccessStatusCode();
        // return await res.Content.ReadFromJsonAsync<Results<int>>();
        return await Results<int>.SuccessAsync(item.Id);
    }

    public async Task<Results<bool>> Remove(int id)
    {
        // var exists = _forecasts.First(m => m.Id == id);
        // _forecasts.Remove(exists);
        return await Results<bool>.SuccessAsync();
    }
}

public class WeatherForecast
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public string Summary { get; set; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudMvvMKit.ViewComponents;

public interface ILoadingIndicator
{
    Task<IMudDialogInstance> Show();
}

public interface IViewModelBase
{
    void OnInitialize();
    Func<string, object, Task> OnClick { get; set; }
}

public abstract class ViewModelBase : IViewModelBase
{
    public abstract void OnInitialize();
    public Func<string, object, Task> OnClick { get; set; }
}

public interface IListViewModelBase<TModel>: IViewModelBase
{
    Func<GridState<TModel>, Task<GridData<TModel>>> OnServerLoad { get; set; }
}

public interface IFormViewModelBase<TModel> : IViewModelBase
{
    TModel Model { get; set; }
    Func<Task<TModel>> OnServerLoad { get; set; }
}

public abstract class MudViewComponent: MudComponentBase
{
    protected const int Delay = 500;
    
    [Inject] public ILoadingIndicator LoadingIndicator { get; set; }
    
    public Func<string, object, Task> OnClick { get; set; }
    public virtual async Task Click(string id, object item)
    {
        if (LoadingIndicator.xIsEmpty()) throw new ApplicationException("Loading indicator is empty");
        
        var instance = await LoadingIndicator.Show();
        await OnClick(id, item);
        await Task.Delay(Delay);
        instance.Close();
    }
    
    protected sealed override void OnInitialized() {OnViewInitialized();}
    protected sealed override async Task OnInitializedAsync()
    {
        await OnViewInitializedAsync();
    }

    protected sealed override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
    }
    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        await OnViewAfterRenderAsync(firstRender);
    }    

    protected virtual void OnViewInitialized()
    {
    }

    protected virtual Task OnViewInitializedAsync()
    {
        return Task.CompletedTask;
    }

    protected virtual void OnViewAfterRender(bool firstRender)
    {
        
    }

    protected virtual Task OnViewAfterRenderAsync(bool firstRender)
    {
        return Task.CompletedTask;
    }

    protected virtual Task OnRetrieve()
    {
        return Task.CompletedTask;
    }
}

public abstract class MudListViewComponent<TViewModel, TModel> : MudViewComponent
    where TViewModel : IListViewModelBase<TModel>
{
    public TViewModel ViewModel { get; set; }
    public virtual async Task<GridData<TModel>> ServerReload(GridState<TModel> state)
    {
        if (LoadingIndicator.xIsEmpty()) throw new ApplicationException("Loading indicator is empty");
        var result = await ViewModel.OnServerLoad(state);
        await Task.Delay(Delay);
        return result;
    }

    protected override void OnViewInitialized()
    {
        ViewModel.OnInitialize();
    }
}

public abstract class MudFormViewComponent<TViewModel, TModel> : MudViewComponent
    where TViewModel : IFormViewModelBase<TModel>
{
    public TViewModel ViewModel { get; set; }

    protected sealed override async Task OnViewAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ViewModel.OnServerLoad();
            await InvokeAsync(StateHasChanged);
        }
    }
}

public abstract class MudListViewModel<TModel> : ViewModelBase, IListViewModelBase<TModel>
{
    public Func<GridState<TModel>, Task<GridData<TModel>>> OnServerLoad { get; set; }
}

public abstract class MudFormViewModel<TModel> : ViewModelBase, IFormViewModelBase<TModel>
{
    public TModel Model { get; set; }

    public Func<string, object, Task> OnClick { get; set; }
    public Func<Task<TModel>> OnServerLoad { get; set; }
}

public class WeatherListViewModel : MudListViewModel<WeatherDto>
{
    public override void OnInitialize()
    {
        OnServerLoad = async (state) =>
        {
            await Task.Delay(500);
            return new GridData<WeatherDto>();
        };
        OnClick = (type, item) =>
        {
            if (type == "insert")
            {
                
            }
            else if (type == "update")
            {
                
            }
            else if (type == "delete")
            {
                
            }
            else if (type == "dialog")
            {
                
            }
            else if (type == "navigate")
            {
                
            }
            
            return Task.CompletedTask;
        };
    }
}

public class WeatherFormViewModel : MudFormViewModel<WeatherDto>
{
    public override void OnInitialize()
    {
        OnServerLoad = async () =>
        {
            await Task.Delay(500);
            return new WeatherDto();
        };
        OnClick = (type, item) =>
        {
            if (type == "insert")
            {
                
            }
            else if (type == "update")
            {
                
            }
            else if (type == "delete")
            {
                
            }
            else if (type == "dialog")
            {
                
            }
            else if (type == "navigate")
            {
                
            }

            return Task.CompletedTask;
        };
    }
}

public class WeatherDto;
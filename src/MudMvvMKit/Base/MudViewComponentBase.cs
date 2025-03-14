using System.ComponentModel;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using MudMvvMKit.ViewComponents;

namespace MudMvvMKit.Base;

public abstract class MudViewComponentBase : MudComponentBase, IDisposable, IAsyncDisposable, IBrowserViewportObserver
{
    protected const int Delay = 500;
    [CascadingParameter] protected INotifyPropertyChanged AppState { get; set; }
    
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public ISnackbar Snackbar { get; set; }
    [Inject] public IBrowserViewportService BrowserViewportService { get; set; } = null!;
    [Inject] public NavigationManager NavManager { get; set; } = null!;
    [Inject] protected IJSRuntime JSRuntime { get; set; }
    
    protected Breakpoint ViewBreakpoint;
    protected List<Breakpoint> ViewBreakpoints = new();
    
    Guid IBrowserViewportObserver.Id { get; } = Guid.NewGuid();
    ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
    {
        ReportRate = 250,
        NotifyOnBreakpointOnly = true
    };

    #region [orignal lifecycle]

    protected sealed override void OnInitialized()
    {
        OnViewInitialized();
        if (AppState.xIsNotEmpty())
        {
            AppState.PropertyChanged += AppStateOnPropertyChanged;    
        }
    }
    protected sealed override async Task OnInitializedAsync()
    {
        await BrowserViewportService.SubscribeAsync(this);

        await OnViewInitializedAsync();
    }
    protected sealed override void OnAfterRender(bool firstRender)
    {
        OnViewAfterRender(firstRender);
    }
    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        await OnViewAfterRenderAsync(firstRender);
    }

    #endregion

    #region [custom lifecycle]

    protected virtual void OnViewInitialized() { }
    protected virtual Task OnViewInitializedAsync()
    {
        return Task.CompletedTask;
    }
    protected virtual void OnViewAfterRender(bool firstRender)
    {
        
    }
    protected virtual Task OnViewAfterRenderAsync(bool firstRender) { return Task.CompletedTask; }    

    #endregion
    
    public Task NotifyBrowserViewportChangeAsync(BrowserViewportEventArgs browserViewportEventArgs)
    {
        if (browserViewportEventArgs.IsImmediate)
        {
            ViewBreakpoint = browserViewportEventArgs.Breakpoint;
        }
        else
        {
            ViewBreakpoints.Add(browserViewportEventArgs.Breakpoint);
        }

        return InvokeAsync(StateHasChanged);
    }

    protected virtual async Task HistoryBack()
    {
        await JSRuntime.InvokeVoidAsync("window.history.back");
    }

    protected virtual async Task HistoryForward()
    {
        await JSRuntime.InvokeVoidAsync("window.history.forward");
    }
    
    private void AppStateOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }
    
    protected async Task<IDialogReference> ShowProgressDialog()
    {
        var dlgOption = new DialogOptions()
        {
            CloseButton = false,
            CloseOnEscapeKey = true,
            BackdropClick = false,
            Position = DialogPosition.Center,
            NoHeader = true
        };
        return await this.DialogService.ShowAsync<ProgressDialog>(null, dlgOption);
    }

    public virtual void Dispose()
    {
        if (BrowserViewportService is IDisposable browserViewportServiceDisposable)
            browserViewportServiceDisposable.Dispose();
        else if (BrowserViewportService != null)
            _ = BrowserViewportService.DisposeAsync().AsTask();

        if (AppState.xIsNotEmpty())
        {
            AppState.PropertyChanged -= AppStateOnPropertyChanged;    
        }
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (BrowserViewportService != null) await BrowserViewportService.DisposeAsync();
        if (AppState.xIsNotEmpty())
        {
            AppState.PropertyChanged -= AppStateOnPropertyChanged;    
        }
    }
    
    protected void NavigateToUrl(string url)
    {
        this.NavManager.NavigateTo(url);
    }

    protected void NavigateToUrlObject(string url, string serializeString)
    {
        //save model into local storage, after next page load model from local storage.
        this.NavManager.NavigateTo(url, new NavigationOptions()
        {
            HistoryEntryState = serializeString
        });
    }
    
    protected T GetUrlObject<T>()
    {
        return this.NavManager.HistoryEntryState.xDeserialize<T>();
    }
}

public abstract class MudViewComponentBase<TModel, TViewModel> : MudViewComponentBase
where TModel : class, new()
where TViewModel : IMudViewModelBase
{
    public TModel SelectedItem { get; set; } = new();
    
    [Inject] public TViewModel ViewModel { get; set; }
    
    public virtual async Task Click(string id, object item)
    {
        this.SelectedItem = (TModel)item;
        
        if (ViewModel.OnClick.xIsEmpty()) return;
        
        await ViewModel.OnClick(id, item);
    }
}

/*
 * MudComponentBase
 *        -> MudViewComponentBase
 *                   -> MudViewComponentBase<TModel, TViewModel>
 *                             -> MudListViewComponent
 *                             -> MudDetailViewComponent
 *
 * View -> ListView <- MudListViewComponent
 *      -> DetailView <- MudDetailViewComponent
 *      -> NoStateView <- MudViewComponentBase, MudViewComponentBase<TModel, TViewModel>
 *
 * ListView <- MudListViewModel
 * DetailView <- MudDetailViewModel
*/
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;
using MudComposite.Components;

namespace MudComposite.ViewComponents;

public abstract class MudViewComponentBase : MudComponentBase, IDisposable, IAsyncDisposable, IBrowserViewportObserver
{
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public IBrowserViewportService BrowserViewportService { get; set; } = null!;
    [Inject] public NavigationManager NavManager { get; set; } = null!;
    
    protected Breakpoint ViewBreakpoint;
    protected List<Breakpoint> ViewBreakpoints = new();
    
    Guid IBrowserViewportObserver.Id { get; } = Guid.NewGuid();
    ResizeOptions IBrowserViewportObserver.ResizeOptions { get; } = new()
    {
        ReportRate = 250,
        NotifyOnBreakpointOnly = true
    };

    protected sealed override void OnInitialized()
    {
        OnViewInitialized();
    }
    protected virtual void OnViewInitialized() { }

    protected sealed override async Task OnInitializedAsync()
    {
        await BrowserViewportService.SubscribeAsync(this);

        await OnViewInitializedAsync();
    }
    protected virtual Task OnViewInitializedAsync()
    {
        return Task.CompletedTask;
    }

    protected sealed override void OnAfterRender(bool firstRender)
    {
        OnViewAfterRender(firstRender);
    }
    protected virtual void OnViewAfterRender(bool firstRender)
    {
    }

    protected sealed override async Task OnAfterRenderAsync(bool firstRender)
    {
        await OnViewAfterRenderAsync(firstRender);
    }
    protected virtual Task OnViewAfterRenderAsync(bool firstRender) { return Task.CompletedTask; }

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
    
    [Inject] protected IJSRuntime JSRuntime { get; set; }

    protected virtual async Task Cancel()
    {
        await JSRuntime.InvokeVoidAsync("window.history.back");
    }

    protected virtual async Task Forward()
    {
        await JSRuntime.InvokeVoidAsync("window.history.forward");
    }

    public void Dispose()
    {
        if (BrowserViewportService is IDisposable browserViewportServiceDisposable)
            browserViewportServiceDisposable.Dispose();
        else if (BrowserViewportService != null)
            _ = BrowserViewportService.DisposeAsync().AsTask();
    }

    public async ValueTask DisposeAsync()
    {
        if (BrowserViewportService != null) await BrowserViewportService.DisposeAsync();
    }
}
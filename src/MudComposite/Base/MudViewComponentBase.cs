using System.ComponentModel;
using System.Security.Claims;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Services;

namespace MudComposite.Base;

public abstract class MudViewComponentBase : MudComponentBase, IDisposable, IAsyncDisposable, IBrowserViewportObserver
{
    [CascadingParameter] protected CultureState CultureState { get; set; }
    
    [Inject] public IDialogService DialogService { get; set; }
    [Inject] public IBrowserViewportService BrowserViewportService { get; set; } = null!;
    [Inject] public NavigationManager NavManager { get; set; } = null!;
    [Inject] protected IJSRuntime JSRuntime { get; set; }
    [Inject] protected AuthenticationStateProvider AuthStateProvider { get; set; }
    
    protected Breakpoint ViewBreakpoint;
    protected List<Breakpoint> ViewBreakpoints = new();
    protected UserSession UserSession;
    
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
        if (CultureState.xIsNotEmpty())
        {
            CultureState.PropertyChanged += CultureStateOnPropertyChanged;    
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
        if (firstRender)
        {
            this.UserSession = await GetUserSession();
        }
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

    protected virtual async Task Cancel()
    {
        await JSRuntime.InvokeVoidAsync("window.history.back");
    }

    protected virtual async Task Forward()
    {
        await JSRuntime.InvokeVoidAsync("window.history.forward");
    }

    protected async Task<UserSession> GetUserSession()
    {
        var state = await this.AuthStateProvider.GetAuthenticationStateAsync();
        var userId = state.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
        var email = state.User.Claims.First(m => m.Type == ClaimTypes.Email).Value;
        var name = state.User.Claims.First(m => m.Type == ClaimTypes.Name).Value;
        var key = state.User.Claims.First(m => m.Type == ClaimTypes.PrimarySid).Value;
        var phone = state.User.Claims.First(m => m.Type == ClaimTypes.MobilePhone).Value;
        var role = state.User.Claims.First(m => m.Type == ClaimTypes.Role).Value;

        return new UserSession()
        {
            UserId = userId,
            Email = email,
            Name = name,
            Role = role,
            UserKey = key,
            Phone = phone,
        };
    }
    
    private void CultureStateOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        StateHasChanged();
    }

    public virtual void Dispose()
    {
        if (BrowserViewportService is IDisposable browserViewportServiceDisposable)
            browserViewportServiceDisposable.Dispose();
        else if (BrowserViewportService != null)
            _ = BrowserViewportService.DisposeAsync().AsTask();

        if (CultureState.xIsNotEmpty())
        {
            CultureState.PropertyChanged -= CultureStateOnPropertyChanged;    
        }
    }

    public virtual async ValueTask DisposeAsync()
    {
        if (BrowserViewportService != null) await BrowserViewportService.DisposeAsync();
        if (CultureState.xIsNotEmpty())
        {
            CultureState.PropertyChanged -= CultureStateOnPropertyChanged;    
        }
    }
}
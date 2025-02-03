using System.Security.Claims;
using eXtensionSharp;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudComposite.ViewComponents;

namespace MudComposite.Base;

public abstract class MudViewModelCore
{
    protected const int Delay = 500;
    
    protected ISnackbar SnackBar;
    protected IDialogService DialogService;

    protected MudViewModelCore(IDialogService dialogService, ISnackbar snackbar)
    {
        this.DialogService = dialogService;
        this.SnackBar = snackbar;
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
}

public interface IMudViewModelBase
{
    Func<string, object, Task> OnClick { get; set; }
    Task Click(string id, object item);
    Task<UserSession> GetUserSession();
}

public abstract class MudViewModelBase : MudViewModelCore, IMudViewModelBase
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    protected MudViewModelBase(IDialogService dialogService, ISnackbar snackbar, AuthenticationStateProvider authenticationStateProvider) : base(dialogService, snackbar)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }
    
    public Func<string, object, Task> OnClick { get; set; }

    public virtual async Task Click(string id, object item)
    {
        if (OnClick.xIsEmpty()) return;

        await OnClick(id, item);
    }

    public async Task<UserSession> GetUserSession()
    {
        var state = await _authenticationStateProvider.GetAuthenticationStateAsync();
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
    

}
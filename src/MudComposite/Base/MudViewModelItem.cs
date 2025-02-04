using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudComposite.Base;

public class MudViewModelItem
{
    public readonly IDialogService DialogService;
    public readonly ISnackbar Snackbar;
    public readonly NavigationManager NavigationManager;

    public MudViewModelItem(IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager)
    {
        DialogService = dialogService;
        Snackbar = snackbar;
        NavigationManager = navigationManager;
    }
}
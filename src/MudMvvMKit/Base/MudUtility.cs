using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudMvvMKit.Base;

public class MudUtility
{
    public readonly IDialogService DialogService;
    public readonly ISnackbar Snackbar;
    public readonly NavigationManager NavigationManager;

    public MudUtility(IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager)
    {
        DialogService = dialogService;
        Snackbar = snackbar;
        NavigationManager = navigationManager;
    }
}
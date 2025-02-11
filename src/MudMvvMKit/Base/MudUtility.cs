using BlazorTrivialJs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudMvvMKit.Base;

public class MudUtility
{
    public readonly IDialogService DialogService;
    public readonly ISnackbar Snackbar;
    public readonly NavigationManager NavigationManager;
    public readonly ITrivialJs TrivialJs;

    public MudUtility(IDialogService dialogService, ISnackbar snackbar, NavigationManager navigationManager, ITrivialJs trivialJs)
    {
        DialogService = dialogService;
        Snackbar = snackbar;
        NavigationManager = navigationManager;
        TrivialJs = trivialJs;
    }
}
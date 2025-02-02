using eXtensionSharp;
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
}

public abstract class MudViewModelBase : MudViewModelCore, IMudViewModelBase
{
    protected MudViewModelBase(IDialogService dialogService, ISnackbar snackbar) : base(dialogService, snackbar)
    {
    }
    
    public Func<string, object, Task> OnClick { get; set; }

    public virtual async Task Click(string id, object item)
    {
        if (OnClick.xIsEmpty()) return;

        await OnClick(id, item);
    }
}
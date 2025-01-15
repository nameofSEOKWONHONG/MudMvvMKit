using MudBlazor;

namespace MudComposite;

public abstract class MudViewCompositeBase
{
    protected const int Delay = 500;
    
    protected ISnackbar SnackBar;
    protected IDialogService DialogService;

    protected MudViewCompositeBase(IDialogService dialogService, ISnackbar snackbar)
    {
        this.DialogService = dialogService;
        this.SnackBar = snackbar;
    }
}
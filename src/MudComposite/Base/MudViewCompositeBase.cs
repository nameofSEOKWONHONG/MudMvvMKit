using MudBlazor;
using MudComposite.ViewComponents;

namespace MudComposite.Base;

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
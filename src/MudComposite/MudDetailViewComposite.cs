using MudBlazor;

namespace MudComposite;

public abstract class MudDetailViewComposite<TModel> : MudViewCompositeBase, IMudDetailViewComposite<TModel>
    where TModel : class, new()
{
    protected MudDetailViewComposite(IDialogService dialogService, ISnackbar snackbar) : base(dialogService, snackbar)
    {
    }
}
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.DetailView;

namespace MudComposite.ViewComponents;

public abstract class MudDetailViewComponent<TParameter, TViewModel> : MudViewComponentBase
    where TViewModel : IMudDetailViewModel<TParameter>
{
    [Inject] protected TViewModel ViewModel { get; set; }
    protected TParameter Parameter { get; set; }

    protected virtual async Task Summit()
    {
        if (ViewModel.OnSubmit.xIsNotEmpty())
        {
            await ViewModel.OnSubmit();
        }
    }

    protected virtual async Task Retrieve()
    {
        if (ViewModel.OnRetrieve.xIsNotEmpty())
        {
            await ViewModel.OnRetrieve();
        }
    }
}
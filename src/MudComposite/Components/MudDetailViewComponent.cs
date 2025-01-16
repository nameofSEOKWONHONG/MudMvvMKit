using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudComposite.Composites.DetailView;
using MudComposite.ViewComponents;

namespace MudComposite.Components;

public abstract class MudDetailViewComponent<TParameter, TComposite> : MudViewComponentBase
    where TComposite : IMudDetailViewComposite<TParameter>
{
    [Inject] protected TComposite Composite { get; set; }
    protected TParameter Parameter { get; set; }

    protected virtual async Task Summit()
    {
        if (Composite.OnSubmit.xIsNotEmpty())
        {
            await Composite.OnSubmit();
        }
    }

    protected virtual async Task Retrieve()
    {
        if (Composite.OnRetrieve.xIsNotEmpty())
        {
            await Composite.OnRetrieve();
        }
    }
}
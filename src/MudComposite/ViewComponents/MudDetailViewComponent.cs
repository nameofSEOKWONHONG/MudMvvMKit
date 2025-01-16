using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.DetailView;

namespace MudComposite.ViewComponents;

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
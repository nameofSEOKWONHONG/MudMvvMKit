using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudComposite.Composites.DetailView;
using MudComposite.ViewComponents;

namespace MudComposite.Components;

public abstract class MudDetailViewComponent<TModel, TComposite> : MudViewComponentBase
    where TComposite : IMudDetailViewComposite<TModel>
{
    [Inject] protected TComposite ViewComposite { get; set; }

    protected virtual async Task Summit()
    {
        if (ViewComposite.OnSubmit.xIsNotEmpty())
        {
            await ViewComposite.OnSubmit();
        }
    }
}
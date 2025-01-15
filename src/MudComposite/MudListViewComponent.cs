using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudComposite;

public abstract class MudListViewComponent<TModel, TSearchModel, TComposite> : MudComponentBase
    where TComposite : IMudListViewComposite<TModel, TSearchModel>
{
    [Inject] protected TComposite ViewComposite { get; set; }
}


using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace MudComposite;

public abstract class MudDetailViewComponent<TModel, TComposite> : MudComponentBase
    where TComposite : IMudDetailViewComposite<TModel>
{
    [Inject] protected TComposite ViewComposite { get; set; }
}
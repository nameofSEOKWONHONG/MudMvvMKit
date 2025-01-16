using Microsoft.AspNetCore.Components;
using MudComposite.Composites.ListView;
using MudComposite.ViewComponents;

namespace MudComposite.Components;

public abstract class MudListViewComponent<TModel, TSearchModel, TComposite> : MudViewComponentBase
    where TComposite : IMudDataGridComposite<TModel, TSearchModel>
{
    [Inject] protected TComposite Composite { get; set; }
}


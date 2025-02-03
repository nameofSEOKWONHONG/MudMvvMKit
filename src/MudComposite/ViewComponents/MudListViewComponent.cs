using System.Text.Json;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.ListView;

namespace MudComposite.ViewComponents;

public abstract class MudListViewComponent<TModel, TSearchModel, TViewModel> : MudViewComponentBase
    where TViewModel : IMudDataGridViewModel<TModel, TSearchModel>
    where TSearchModel : class
    where TModel : class
{
    [Inject] protected TViewModel ViewModel { get; set; }
}


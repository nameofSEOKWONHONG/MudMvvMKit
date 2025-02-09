using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.DetailView;

namespace MudComposite.ViewComponents;

public abstract class MudDetailViewComponent<TParameter, TModel, TViewModel> : MudViewComponentBase
    where TViewModel : IMudDetailViewModel<TParameter, TModel>
{
    [Inject] protected TViewModel ViewModel { get; set; }
}
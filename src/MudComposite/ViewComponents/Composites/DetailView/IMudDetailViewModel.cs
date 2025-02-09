using MudBlazor;
using MudComposite.Base;

namespace MudComposite.ViewComponents.Composites.DetailView;

public interface IMudDetailViewModel<TParameter, TModel>: IMudViewModelBase
{
    MudForm MudForm { get; set; }
    TModel RetrievedItem { get; set; }
    TParameter Parameter { get; set; }
    
    #region [event]

    Func<Task<Results>> OnSubmit { get; set; }
    Func<Task<Results>> OnRetrieve { get; set; }    

    #endregion

    Task Retrieve();
    Task Submit();
}
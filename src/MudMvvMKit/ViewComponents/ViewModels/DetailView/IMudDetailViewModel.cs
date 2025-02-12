using FluentValidation;
using MudMvvMKit.Base;

namespace MudMvvMKit.ViewComponents.ViewModels.DetailView;

public interface IMudDetailViewModel<TParameter, TModel>: IMudViewModelBase
{
    TModel RetrievedItem { get; set; }
    TParameter Parameter { get; set; }
    
    #region [event]

    Func<Task<Results>> OnSubmit { get; set; }
    Func<Task<Results>> OnRetrieve { get; set; }    

    #endregion
}

public interface IFormValidator
{
    Task<IEnumerable<string>> ValidateValue(object model, string propertyName);
}
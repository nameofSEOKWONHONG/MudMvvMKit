using eXtensionSharp;
using FluentValidation;
using MudBlazor;
using MudComposite.Base;
using Severity = MudBlazor.Severity;

namespace MudComposite.ViewComponents.Composites.DetailView;

public abstract class MudDetailViewModel<TParameter, TModel> : MudViewModelBase, IMudDetailViewModel<TParameter, TModel>
    where TModel : class, new()
{
    protected MudDetailViewModel()
    {
    }
    
    public TModel RetrievedItem { get; set; } = new();
    public TParameter Parameter { get; set; }

    public Func<Task<Results>> OnSubmit { get; set; }
    public Func<Task<Results>> OnRetrieve { get; set; }

    protected string[] Errors = [];

    protected bool? IsSingleError;

}
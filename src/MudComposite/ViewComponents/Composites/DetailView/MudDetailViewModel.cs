using eXtensionSharp;
using FluentValidation;
using MudBlazor;
using MudComposite.Base;
using Severity = MudBlazor.Severity;

namespace MudComposite.ViewComponents.Composites.DetailView;

public abstract class MudDetailViewModel<TParameter, TModel> : MudViewModelBase, IMudDetailViewModel<TParameter, TModel>
    where TModel : class, new()
{
    protected MudDetailViewModel(MudViewModelItem utility) : base(utility)
    {
    }

    public MudForm MudForm { get; set; }
    public TModel RetrievedItem { get; set; } = new();
    public TParameter Parameter { get; set; }

    public Func<Task<Results>> OnSubmit { get; set; }
    public Func<Task<Results>> OnRetrieve { get; set; }

    protected string[] Errors = [];

    protected bool? IsSingleError;

    public virtual async Task Retrieve()
    {
        if(OnRetrieve.xIsEmpty()) return;
        
        var dlg = await this.ShowProgressDialog();
        var result = await OnRetrieve();
        await Task.Delay(Delay);
        dlg.Close();

        this.Utility.Snackbar.Add(result.Messages.xJoin(), result.Succeeded ? Severity.Success : Severity.Error);        
    }

    public virtual async Task Submit()
    {
        if(this.MudForm.xIsEmpty()) throw new Exception("MudForm is empty");
        
        if (OnSubmit.xIsEmpty()) throw new Exception("OnSubmit is empty");
        
        await this.MudForm.Validate();
        
        if (this.MudForm.IsValid.xIsFalse())
        {
            if (IsSingleError.xIsNotEmpty())
            {
                if (IsSingleError.HasValue)
                {
                    var error = this.MudForm.Errors.xFirst();
                    this.Utility.Snackbar.Add(error, Severity.Error);                
                }
                else
                {
                    this.Utility.Snackbar.Add(this.MudForm.Errors.xJoin(Environment.NewLine), Severity.Error);
                }                
            }
            return;
        }

        var dlg = await this.ShowProgressDialog();
        var result = await OnSubmit();
        await Task.Delay(Delay);
        dlg.Close();

        this.Utility.Snackbar.Add(result.Messages.xJoin(), result.Succeeded ? Severity.Success : Severity.Error);
    }
}
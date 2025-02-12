using eXtensionSharp;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudMvvMKit.Base;
using MudMvvMKit.ViewComponents.ViewModels.DetailView;
using Severity = MudBlazor.Severity;

namespace MudMvvMKit.ViewComponents;

public abstract class MudDetailViewComponent<TParameter, TModel, TViewModel> : MudViewComponentBase<TModel, TViewModel>
    where TModel : class, new()
    where TViewModel : IMudDetailViewModel<TParameter, TModel>
{
    [Inject] protected TViewModel ViewModel { get; set; }
    protected MudForm Form { get; set; }
    
    public IFormValidator FormValidator { get; set; }

    protected override void OnViewInitialized()
    {
        
    }

    public virtual async Task Retrieve()
    {
        if(ViewModel.OnRetrieve.xIsEmpty()) return;
        
        var dlg = await this.ShowProgressDialog();
        var result = await ViewModel.OnRetrieve();
        await Task.Delay(Delay);
        dlg.Close();

        this.Snackbar.Add(result.Messages.xJoin(), result.Succeeded ? Severity.Success : Severity.Error);
    }
    
    public virtual async Task Submit()
    {
        if(this.Form.xIsEmpty()) throw new Exception("MudForm is empty");
        
        if (this.ViewModel.OnSubmit.xIsEmpty()) throw new Exception("OnSubmit is empty");
        
        await this.Form.Validate();
        
        if (this.Form.IsValid.xIsFalse())
        {
            this.Snackbar.Add(this.Form.Errors.xJoin(Environment.NewLine), Severity.Error);
            return;
        }

        var dlg = await this.ShowProgressDialog();
        var result = await this.ViewModel.OnSubmit();
        await Task.Delay(Delay);
        dlg.Close();

        this.Snackbar.Add(result.Messages.xJoin(), result.Succeeded ? Severity.Success : Severity.Error);
        await Task.Delay(Delay);

        await TrivialJs.GoBack();
    }
}
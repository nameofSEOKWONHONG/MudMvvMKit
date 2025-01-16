namespace MudComposite.Composites.DetailView;

public interface IMudDetailViewComposite<TRetrieved>
{
    TRetrieved RetrieveItem { get; set; }
    
    #region [event]

    Func<Task<Results>> OnSubmit { get; set; }
    Func<Task<Results>> OnRetrieve { get; set; }    

    #endregion
}
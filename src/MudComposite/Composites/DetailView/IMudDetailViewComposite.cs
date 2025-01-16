namespace MudComposite.Composites.DetailView;

public interface IMudDetailViewComposite<TRetrieved>
{
    TRetrieved RetrievedItem { get; set; }
    
    #region [event]

    Func<Task<Results>> OnSubmit { get; set; }
    Func<Task<Results>> OnRetrieve { get; set; }    

    #endregion

    #region [method]

    Task Retrieve();
    Task Submit();        

    #endregion


}
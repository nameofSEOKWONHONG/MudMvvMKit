using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudComposite.Base;

namespace MudComposite.ViewComponents.Composites.ListView;

public abstract class MudDataGridViewModel<TModel, TSearchModel> : MudViewModelBase, IMudDataGridViewModel<TModel, TSearchModel>
    where TModel : class, new()
    where TSearchModel : class, new()
{
    protected readonly MudUtility MudUtility;

    #region [public variables]

    public TModel SelectedItem { get; set; } = new();
    public List<TModel> SelectedItems { get; set; } = new();
    public TSearchModel SearchModel { get; set; }

    #endregion

    #region [public func's]

    /// <summary>
    /// Implement ServerReload event
    /// </summary>
    public Func<GridState<TModel>, Task<GridData<TModel>>> OnServerReload { get; set; }
    
    /// <summary>
    /// Implement Remove event
    /// </summary>
    public Func<TModel, Task<Results>> OnRemove { get; set; }   
    public Func<TModel, Task<TModel>> OnSaveBefore { get; set; }
    public Func<TModel, Task<Results>> OnSave { get; set; }
    public Func<TModel, Task<Results>> OnSaveAfter { get; set; }

    #endregion

    #region [protected variables]

    public MudDataGrid<TModel> DataGrid { get; set; }
    protected MudTable<TModel> Table  { get; }
    
    #endregion

    
    public MudDataGridViewModel(MudUtility mudUtility)
    {
        MudUtility = mudUtility;
        this.SearchModel = new TSearchModel();
    }

    /// <summary>
    /// initialized in 'OnAfterRender'
    /// </summary>
    /// <param name="dataGrid"></param>
    public virtual void Initialize()
    {
    }

    public virtual Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}


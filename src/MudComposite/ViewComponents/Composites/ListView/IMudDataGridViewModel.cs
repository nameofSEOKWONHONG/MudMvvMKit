using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudComposite.Base;

namespace MudComposite.ViewComponents.Composites.ListView;

public interface IMudDataGridViewModel<TModel, TSearchModel> : IMudViewModelBase
    where TModel : class, new()
{
    #region [property]

    TModel SelectedItem { get; set; }
    List<TModel> SelectedItems { get; set; }
    TSearchModel SearchModel { get; set; }    

    MudDataGrid<TModel> DataGrid { get; set; }
    
    #endregion

    #region [event]

    Func<GridState<TModel>, Task<GridData<TModel>>> OnServerReload { get; set; }
    Func<TModel, Task<Results>> OnRemove { get; set; }
    Func<TModel, Task<TModel>> OnSaveBefore { get; set; }
    #endregion

    #region [method]

    Task<GridData<TModel>> ServerReload(GridState<TModel> state);
    Task ReloadServerData();
    Task Remove(TModel item);
    Task Save(TModel item);
    Task SearchKeyUp(KeyboardEventArgs e);
    void DataGridRowClick(DataGridRowClickEventArgs<TModel> obj);
    string RowStyleFunc(TModel item, int id);
    Task SearchClear();
    void GoDetail();

    void Initialize();
    Task InitializeAsync();
    #endregion
}
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace MudComposite.ViewComponents.Composites.ListView;

public interface IMudDataGridComposite<TModel, TSearchModel>
{
    #region [property]

    TModel SelectedItem { get; set; }
    List<TModel> SelectedItems { get; set; }
    TSearchModel SearchModel { get; set; }    

    #endregion

    #region [event]

    Func<GridState<TModel>, Task<GridData<TModel>>> OnServerReload { get; set; }
    Func<TModel, Task<Results>> OnRemove { get; set; }    

    #endregion

    #region [method]

    Task<GridData<TModel>> ServerReload(GridState<TModel> state);
    Task Remove(TModel item);
    Task SearchKeyUp(KeyboardEventArgs e);
    void DataGridRowClick(DataGridRowClickEventArgs<TModel> obj);
    string RowStyleFunc(TModel item, int id);
    Task SearchClear();
    void SetUp(MudDataGrid<TModel> dataGrid);
    void GoDetail();

    void Initialize();

    #endregion
}
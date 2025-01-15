using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace MudComposite;

public interface IMudListViewComposite<TModel, TSearchModel>
{
    Func<GridState<TModel>, Task<GridData<TModel>>> OnServerReload { get; set; }
    Func<TModel, Task<Results>> OnRemove { get; set; }
    
    TModel SelectedItem { get; set; }
    List<TModel> SelectedItems { get; set; }
    TSearchModel SearchModel { get; set; }

    Task<GridData<TModel>> ServerReload(GridState<TModel> state);
    Task Remove(TModel item);
    Task SearchKeyUp(KeyboardEventArgs e);
    void DataGridRowClick(DataGridRowClickEventArgs<TModel> obj);
    string RowStyleFunc(TModel item, int id);
    Task SearchClear();
    void SetUp(MudDataGrid<TModel> dataGrid);
}




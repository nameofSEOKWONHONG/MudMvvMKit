using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudComposite.Base;

namespace MudComposite.ViewComponents.Composites.ListView;

public abstract class MudDataGridComposite<TModel, TSearchModel> : MudViewCompositeBase, IMudDataGridComposite<TModel, TSearchModel>
    where TModel : class, new()
    where TSearchModel : class, new()
{   
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

    protected MudDataGrid<TModel> DataGrid { get; set; }

    
    #endregion

    protected NavigationManager NavManager;
    public MudDataGridComposite(IDialogService dialogService,
        ISnackbar snackbar,
        NavigationManager navigationManager) : base(dialogService, snackbar)
    {
        this.SearchModel = new TSearchModel();
        NavManager = navigationManager;
    }
    
    /// <summary>
    /// The grid is initialized in 'OnAfterRender'
    /// </summary>
    /// <param name="dataGrid"></param>
    public void SetUp(MudDataGrid<TModel> dataGrid)
    {
        DataGrid = dataGrid;
    }

    protected MudTable<TModel> Table;
    
    #region [events]

    /// <summary>
    /// Retrieve the grid
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public virtual async Task<GridData<TModel>> ServerReload(GridState<TModel> state)
    {
        if (OnServerReload.xIsEmpty()) return null;
        
        var dlgOption = new DialogOptions()
        {
            CloseButton = false,
            CloseOnEscapeKey = true,
            BackdropClick = false,
            Position = DialogPosition.Center,
            NoHeader = true
        };
        var dlg = await this.DialogService.ShowAsync<ProgressDialog>(null, dlgOption);
        var result = await OnServerReload(state);
        
        await Task.Delay(Delay);
        dlg.Close();

        return result;
    }

    public virtual async Task ReloadServerData()
    {
        if(this.DataGrid.xIsEmpty()) return;
        await this.DataGrid.ReloadServerData();
    }
    
    /// <summary>
    /// Search event linked to the grid
    /// </summary>
    /// <param name="e"></param>
    public virtual async Task SearchKeyUp(KeyboardEventArgs e)
    {
        if (e.Code is "Enter" or "NumpadEnter")
        {
            await this.DataGrid.ReloadServerData();
        }
    }
    
    /// <summary>
    /// Grid row click event
    /// </summary>
    /// <param name="obj"></param>
    public virtual void DataGridRowClick(DataGridRowClickEventArgs<TModel> obj)
    {
        this.SelectedItem = obj.Item;
    }

    /// <summary>
    /// Grid row style event
    /// </summary>
    /// <param name="item"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual string RowStyleFunc(TModel item, int id)
    {
        return item == SelectedItem ? "background-color: #cce4ff;" : string.Empty;
    }
    
    /// <summary>
    /// Search initialization event linked to the grid
    /// </summary>
    public virtual async Task SearchClear()
    {
        this.SearchModel = new TSearchModel();
        
        // CAUTION: Attempted to handle it with parameters, but it did not work as expected, so it was forcibly processed.
        this.DataGrid.CurrentPage = 0;
        
        await Task.Delay(Delay);
        if (!this.DataGrid.Loading)
        {
            await this.DataGrid.ReloadServerData();
        }
    }

    /// <summary>
    /// remove event
    /// </summary>
    /// <param name="item"></param>
    public virtual async Task Remove(TModel item)
    {
        if(OnRemove.xIsEmpty()) return;
        
        var question = await this.DialogService.ShowMessageBox("경고", "선택한 데이터를 삭제 하시겠습니까? (삭제된 데이터는 복구할 수 없습니다.)", "YES", "NO");
        if (question.GetValueOrDefault())
        {
            var dlgOption = new DialogOptions()
            {
                CloseButton = false,
                CloseOnEscapeKey = true,
                BackdropClick = false,
                Position = DialogPosition.Center,
                NoHeader = true
            };
            var dlg = await this.DialogService.ShowAsync<ProgressDialog>(null, dlgOption);
            var result = await OnRemove(item);
            await Task.Delay(Delay);
            dlg.Close();
            
            if (result.Succeeded)
            {
                await this.DataGrid.ReloadServerData();
                this.SnackBar.Add(result.Messages.xJoin(), Severity.Success);
            }
            else
            {
                this.SnackBar.Add(result.Messages.xJoin(), Severity.Error);
            }
        }
    }

    public virtual async Task Save(TModel item)
    {
        if (OnSave.xIsEmpty()) return;
        if (OnSaveBefore.xIsNotEmpty())
        {
            item = await OnSaveBefore(item);
        }

        var dlg = await ShowProgressDialog();
        await OnSave(item);
        await Task.Delay(Delay);
        dlg.Close();

        if (OnSaveAfter.xIsNotEmpty())
        {
            await OnSaveBefore(item);
        }
        else
        {
            await this.DataGrid.ReloadServerData();    
        }
    }

    #endregion

    public virtual void GoDetail()
    {
        
    }

    public virtual void Initialize()
    {
        
    }
}


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
    #region [public variables]

    public TModel SelectedItem { get; set; }
    public List<TModel> SelectedItems { get; set; }
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

    
    public MudDataGridViewModel(MudViewModelItem viewModelItem) : base(viewModelItem)
    {
        this.SearchModel = new TSearchModel();
    }

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
        var dlg = await this.Utility.DialogService.ShowAsync<ProgressDialog>(null, dlgOption);
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

    public override async Task Click(string id, object obj)
    {
        if (OnClick.xIsEmpty()) return;
        
        var item = obj.xAs<TModel>();
        if(item.xIsNotEmpty()) this.SelectedItem = item;

        await OnClick(id, obj);
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
        
        var question = await this.Utility.DialogService.ShowMessageBox("경고", "선택한 데이터를 삭제 하시겠습니까? (삭제된 데이터는 복구할 수 없습니다.)", "YES", "NO");
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
            var dlg = await this.Utility.DialogService.ShowAsync<ProgressDialog>(null, dlgOption);
            var result = await OnRemove(item);
            await Task.Delay(Delay);
            dlg.Close();
            
            if (result.Succeeded)
            {
                await this.DataGrid.ReloadServerData();
                this.Utility.Snackbar.Add(result.Messages.xJoin(), Severity.Success);
            }
            else
            {
                this.Utility.Snackbar.Add(result.Messages.xJoin(), Severity.Error);
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
        if(item.xIsEmpty()) return;

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

    protected void NavigateToUrl(string url)
    {
        this.Utility.NavigationManager.NavigateTo(url);
    }

    protected void NavigateToUrlObject(string url, TModel model)
    {
        //save model into local storage, after next page load model from local storage.
        this.Utility.NavigationManager.NavigateTo(url, new NavigationOptions()
        {
            HistoryEntryState = model.xSerialize()
        });
    }

    protected T GetUrlObject<T>()
    {
        return this.Utility.NavigationManager.HistoryEntryState.xDeserialize<T>();
    }
}


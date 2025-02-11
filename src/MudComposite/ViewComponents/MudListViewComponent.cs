using System.Text.Json;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.ListView;

namespace MudComposite.ViewComponents;

public abstract class MudListViewComponent<TModel, TSearchModel, TViewModel> : MudViewComponentBase<TModel, TViewModel>
    where TViewModel : IMudListViewModel<TModel, TSearchModel>
    where TSearchModel : class, new()
    where TModel : class, new()
{
    [Inject] protected TViewModel ViewModel { get; set; }
    protected MudDataGrid<TModel> DataGrid { get; set; }

    protected override void OnViewAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            ViewModel.Initialize();    
        }
    }

    protected override async Task OnViewAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ViewModel.InitializeAsync();
        }
    }
    
    /// <summary>
    /// Retrieve the grid
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public virtual async Task<GridData<TModel>> ServerReload(GridState<TModel> state)
    {
        if (ViewModel.OnServerReload.xIsEmpty()) return null;
        
        var dlgOption = new DialogOptions()
        {
            CloseButton = false,
            CloseOnEscapeKey = true,
            BackdropClick = false,
            Position = DialogPosition.Center,
            NoHeader = true
        };
        var dlg = await this.DialogService.ShowAsync<ProgressDialog>(null, dlgOption);
        var result = await ViewModel.OnServerReload(state);
        
        await Task.Delay(Delay);
        dlg.Close();

        return result;
    }
    
    public override async Task Click(string id, object obj)
    {
        if (ViewModel.OnClick.xIsEmpty()) return;
        
        var item = obj.xAs<TModel>();
        if(item.xIsNotEmpty()) this.ViewModel.SelectedItem = item;

        await ViewModel.OnClick(id, obj);
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
        this.ViewModel.SelectedItem = obj.Item;
    }

    /// <summary>
    /// Grid row style event
    /// </summary>
    /// <param name="item"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual string RowStyleFunc(TModel item, int id)
    {
        return item == this.ViewModel.SelectedItem ? "background-color: #cce4ff;" : string.Empty;
    }
    
    /// <summary>
    /// Search initialization event linked to the grid
    /// </summary>
    public virtual async Task SearchClear()
    {
        this.ViewModel.SearchModel = new TSearchModel();
        
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
        if(this.ViewModel.OnRemove.xIsEmpty()) return;
        
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
            var result = await this.ViewModel.OnRemove(item);
            await Task.Delay(Delay);
            dlg.Close();
            
            if (result.Succeeded)
            {
                await this.DataGrid.ReloadServerData();
                this.Snackbar.Add(result.Messages.xJoin(), Severity.Success);
            }
            else
            {
                this.Snackbar.Add(result.Messages.xJoin(), Severity.Error);
            }
        }
    }

    public virtual async Task Save(TModel item)
    {
        this.SelectedItem = item;
        
        if (this.ViewModel.OnSave.xIsEmpty()) return;
        if (this.ViewModel.OnSaveBefore.xIsNotEmpty())
        {
            item = await this.ViewModel.OnSaveBefore(item);
        }
        if(item.xIsEmpty()) return;

        var dlg = await ShowProgressDialog();
        await this.ViewModel.OnSave(item);
        await Task.Delay(Delay);
        dlg.Close();

        if (this.ViewModel.OnSaveAfter.xIsNotEmpty())
        {
            await this.ViewModel.OnSaveAfter(item);
        }
        else
        {
            await this.DataGrid.ReloadServerData();    
        }
    }
}


﻿using MudBlazor;
using MudMvvMKit.Base;

namespace MudMvvMKit.ViewComponents.ViewModels.ListView;

public interface IMudListViewModel<TModel, TSearchModel> : IMudViewModelBase
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
    public Func<TModel, Task<TModel>> OnSaveBefore { get; set; }
    public Func<TModel, Task<Results>> OnSave { get; set; }
    public Func<TModel, Task<Results>> OnSaveAfter { get; set; }
    #endregion

    #region [method]

    void Initialize();
    Task InitializeAsync();
    #endregion
}
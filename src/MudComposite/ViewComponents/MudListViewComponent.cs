using System.Text.Json;
using eXtensionSharp;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.ListView;

namespace MudComposite.ViewComponents;

public abstract class MudListViewComponent<TModel, TSearchModel, TViewModel> : MudViewComponentBase
    where TViewModel : IMudDataGridViewModel<TModel, TSearchModel>
    where TSearchModel : class
    where TModel : class, new()
{
    [Inject] protected TViewModel ViewModel { get; set; }
    protected MudDataGrid<TModel> DataGrid { get; set; }

    protected override void OnViewAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            ViewModel.Initialize(DataGrid);    
        }
    }

    protected override async Task OnViewAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await ViewModel.InitializeAsync(DataGrid);
        }
    }
}


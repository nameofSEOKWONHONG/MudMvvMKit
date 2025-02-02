﻿using Microsoft.AspNetCore.Components;
using MudComposite.Base;
using MudComposite.ViewComponents.Composites.ListView;

namespace MudComposite.ViewComponents;

public abstract class MudListViewComponent<TModel, TSearchModel, TViewModel> : MudViewComponentBase
    where TViewModel : IMudDataGridViewModel<TModel, TSearchModel>
{
    [Inject] protected TViewModel ViewModel { get; set; }

    protected virtual void NavigateToUrl(string url)
    {
        NavManager.NavigateTo(url);
    }

    protected virtual void NavigateToUrlObject(string url, TModel model)
    {
        //save model into local storage, after next page load model from local storage.
        NavManager.NavigateTo(url);
    }
}


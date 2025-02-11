using eXtensionSharp;
using MudBlazor;
using MudMvvMKit.ViewComponents;

namespace MudMvvMKit.Base;

public abstract class MudViewModelBase : IMudViewModelBase
{
    protected const int Delay = 500;
    
    protected MudViewModelBase()
    {
    }
    
    public Func<string, object, Task> OnClick { get; set; }
}
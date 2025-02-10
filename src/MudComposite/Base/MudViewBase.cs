using eXtensionSharp;
using MudBlazor;
using MudComposite.ViewComponents;

namespace MudComposite.Base;

public abstract class MudViewModelBase : IMudViewModelBase
{
    protected const int Delay = 500;
    
    protected MudViewModelBase()
    {
    }
    
    public Func<string, object, Task> OnClick { get; set; }
}
namespace MudComposite.Base;

public interface IMudViewModelBase
{
    Func<string, object, Task> OnClick { get; set; }
}
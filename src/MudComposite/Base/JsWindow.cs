using Microsoft.JSInterop;
using MudBlazor;

namespace MudComposite.Base;

public interface IJsWindow
{
    Task CopyClipboard(string text, bool isNotification = true);
}

public class JsWindow: IJsWindow
{
    private readonly IJSRuntime _jsRuntime;
    private readonly ISnackbar _snackbar;

    public JsWindow(IJSRuntime jsRuntime, ISnackbar snackbar)
    {
        _jsRuntime = jsRuntime;
        _snackbar = snackbar;
    }

    public async Task CopyClipboard(string text, bool isNotification = true)
    {
        await _jsRuntime.InvokeVoidAsync("copyClipboard", text);
        
        if(isNotification) 
            _snackbar.Add("copied", Severity.Success);
    }
}
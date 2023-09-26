using SpeakBot.Models;
using Microsoft.JSInterop;

namespace SpeakBot.Services;

public interface ICookieStoreAPIService
{
    Task DeleteAsync(string name);
    Task<IEnumerable<Cookie>> GetAllAsync();
    Task<Cookie> GetAsync(string name);
    Task SetAsync(Cookie cookie);
}

public class CookieStoreAPIService : ICookieStoreAPIService
{
    private readonly IJSRuntime _jSRuntime;

    public CookieStoreAPIService(IJSRuntime jSRuntime)
    {
        _jSRuntime = jSRuntime;
    }

    public async Task<Cookie> GetAsync(string name)
    {
        var ret = await _jSRuntime.InvokeAsync<Cookie>("cookieStore.get", name);
        return ret;
    }

    public async Task<IEnumerable<Cookie>> GetAllAsync()
        => await _jSRuntime.InvokeAsync<IEnumerable<Cookie>>("cookieStore.getAll");

    public async Task SetAsync(Cookie cookie)
        => await _jSRuntime.InvokeVoidAsync("cookieStore.set", cookie);

    public async Task DeleteAsync(string name)
        => await _jSRuntime.InvokeAsync<Cookie>("cookieStore.delete", name);
}

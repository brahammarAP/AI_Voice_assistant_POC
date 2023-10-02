using SpeakBot.Models;
using Microsoft.JSInterop;

namespace SpeakBot.Services;

public interface ICookieAPIService
{
    Task DeleteAsync(string name);
    Task<IEnumerable<Cookie>> GetAllAsync();
    Task<Cookie> GetAsync(string name);
    Task SetAsync(Cookie cookie);
}

public class CookieAPIService : ICookieAPIService
{
    private readonly IJSRuntime _jsRuntime;

    public CookieAPIService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task DeleteAsync(string name)
    {
        await _jsRuntime.InvokeVoidAsync("cookieFunctions.deleteCookie", name);
    }

    public async Task<IEnumerable<Cookie>> GetAllAsync()
    {
        return await _jsRuntime.InvokeAsync<List<Cookie>>("cookieFunctions.getAllCookies");
    }

    public async Task<Cookie> GetAsync(string name)
    {
        return await _jsRuntime.InvokeAsync<Cookie>("cookieFunctions.getCookie", name);
    }

    public async Task SetAsync(Cookie cookie)
    {
        await _jsRuntime.InvokeVoidAsync("cookieFunctions.setCookie", cookie);
    }
}


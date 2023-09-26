using Microsoft.JSInterop;
using System.Text.Json;

namespace SpeakBot.Services;

public interface ILocalStorageAPI
{
    public Task<T> GetItemAsync<T>(string key);
    public Task SetItemAsync<T>(string key, T value);
    public Task RemoveItemAsync(string key);
}

public class LocalStorageAPIService : ILocalStorageAPI
{
    private readonly IJSRuntime jsRuntime;

    public LocalStorageAPIService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task<T> GetItemAsync<T>(string key)
    {
        var json = await jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

        if (json == null) return default;

        return JsonSerializer.Deserialize<T>(json);
    }

    public async Task SetItemAsync<T>(string key, T value)
        => await jsRuntime.InvokeVoidAsync("localStorage.setItem", key, JsonSerializer.Serialize(value));

    public async Task RemoveItemAsync(string key)
        => await jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
}

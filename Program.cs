using SpeakBot;
using SpeakBot.Repository;
using SpeakBot.Repository.IRepository;
using SpeakBot.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(builder.Configuration);

builder.Services
    .AddSingleton<IAzureAiChatServices, AzureAiChatServices>()
    .AddSingleton<IUserSession, UserSession>()
    .AddSingleton<ICookieStoreAPIService, CookieStoreAPIService>();

builder.Services
    .AddScoped<ILocalStorageAPI, LocalStorageAPIService>()
    .AddScoped<IChatHistoryRepository, ChatHistoryRepository>()
    .AddScoped<INavigationServices, NavigationServices>()
    .AddScoped<MobileSwipeService>()
    .AddScoped<IMessageService, MessageService>();

await builder.Build().RunAsync();

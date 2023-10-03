using SpeakBot;
using SpeakBot.Repository;
using SpeakBot.Repository.IRepository;
using SpeakBot.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazor.SubtleCrypto;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSubtleCrypto(opt =>
    opt.Key = "ELE9xOyAyJHCsIPLMbbZHQ7pVy7WUlvZ60y5WkKDGMSw5xh5IM54kUPlycKmHF9VGtYUilglL8iePLwr"
);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton(builder.Configuration);

builder.Services
    .AddSingleton<IAzureAiChatServices, AzureAiChatServices>()
    .AddSingleton<IUserSession, UserSession>()
    .AddSingleton<ICookieAPIService, CookieAPIService>();

builder.Services
    .AddScoped<ILocalStorageAPI, LocalStorageAPIService>()
    .AddScoped<IChatHistoryRepository, ChatHistoryRepository>()
    .AddScoped<INavigationServices, NavigationServices>()
    .AddScoped<IMobileSwipeService,MobileSwipeService>()
    .AddScoped<IMessageService, MessageService>();

await builder.Build().RunAsync();

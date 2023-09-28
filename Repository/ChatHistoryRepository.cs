using ChatBotAI.Models;
using ChatBotAI.Repository.IRepository;
using ChatBotAI.Services;
using Microsoft.JSInterop;
using System.Linq.Expressions;

namespace ChatBotAI.Repository;

public class ChatHistoryRepository : LocalStorageRepository<ChatHistory>, IChatHistoryRepository
{
    public ChatHistoryRepository(IConfiguration configuration, ILocalStorageAPI localStorageAPI, IJSRuntime jSRuntime) : base(configuration, localStorageAPI)
    {
    }

    public async Task<ChatHistory> FavoriseAsync(Expression<Func<ChatHistory, bool>>? filter = null)
    {
        if (filter == null) return null!;

        var query = await GetAllAsync();

        var updateEntity = query.Where(filter.Compile()).FirstOrDefault();

        if (updateEntity == null) return null!;

        var toggleFavorised = (updateEntity.IsLocked) ? false : true;

        updateEntity.IsLocked = toggleFavorised;

        await SaveAsync(query);

        return updateEntity;
    }

    public async Task RemoveItemWhenMoreThanFive(List<ChatHistory> chatHistory)
    {
        List<ChatHistory> newChatHistory = new List<ChatHistory>();

        var notFavoritedChatHistory = chatHistory.Where(history => !history.IsLocked).ToList();

        if (chatHistory.Take(chatHistory.Count - 1).All(history => history.IsLocked))
        {
            throw new Exception("All chats are liked. Please unlike one to create new chat.");
        }

        if (chatHistory.Count() >= 5 && chatHistory.All(history => !history.IsLocked))
        {
            chatHistory.Remove(chatHistory.First());
            newChatHistory = chatHistory;
        }

        if (chatHistory.Count() > 5 && chatHistory.Any(history => history.IsLocked))
        {
            var removeItem = notFavoritedChatHistory.First();
            notFavoritedChatHistory.Remove(removeItem);
            chatHistory.RemoveAll(history => history.Equals(removeItem));
            newChatHistory = chatHistory.Union(notFavoritedChatHistory).ToList();
        }

        await SaveAsync(newChatHistory);

        }
}



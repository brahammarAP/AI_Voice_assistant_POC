using SpeakBot.Models;
using SpeakBot.Repository.IRepository;
using SpeakBot.Services;
using System.Linq.Expressions;

namespace SpeakBot.Repository;

public class ChatHistoryRepository : LocalStorageRepository<ChatHistory>, IChatHistoryRepository
{
    public ChatHistoryRepository(IConfiguration configuration, ILocalStorageAPI localStorageAPI) : base(configuration, localStorageAPI)
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
}

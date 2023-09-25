using ChatBotAI.Models;
using System.Linq.Expressions;

namespace ChatBotAI.Repository.IRepository;

public interface IChatHistoryRepository : ILocalStorageRepository<ChatHistory>
{
    Task<ChatHistory> FavoriseAsync(Expression<Func<ChatHistory, bool>>? filter = null);
}

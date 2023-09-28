using SpeakBot.Models;
using System.Linq.Expressions;

namespace SpeakBot.Repository.IRepository;

public interface IChatHistoryRepository : ILocalStorageRepository<ChatHistory>
{
    Task<ChatHistory> FavoriseAsync(Expression<Func<ChatHistory, bool>>? filter = null);

    Task RemoveItemWhenMoreThanFive(List<ChatHistory> chatHistory);
}

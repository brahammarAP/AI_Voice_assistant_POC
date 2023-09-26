using System.Linq.Expressions;

namespace SpeakBot.Repository.IRepository;

public interface ILocalStorageRepository<T> where T : class
{
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null);
    Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task CreateAsync(ICollection<T> entity);
    Task DeleteAsync(Expression<Func<T, bool>>? filter = null);
    Task SaveAsync(ICollection<T> entity);
}

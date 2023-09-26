using System.Linq.Expressions;

namespace SpeakBot.Repository.IRepository;

public interface IRepository<T> where T : class
{
    Task<T> GetAsync(Expression<Func<T, bool>>? filter = null);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
    Task CreateAsync(T entity);
    Task DeleteAsync(T entity);
    Task SaveAsync();
}

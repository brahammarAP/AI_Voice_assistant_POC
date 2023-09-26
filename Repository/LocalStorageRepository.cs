using SpeakBot.Repository.IRepository;
using SpeakBot.Services;
using System.Linq.Expressions;

namespace SpeakBot.Repository;

public class LocalStorageRepository<T> : ILocalStorageRepository<T> where T : class
{
    private readonly string storageLocation;
    private readonly ILocalStorageAPI localStorageAPI;

    public LocalStorageRepository(IConfiguration configuration, ILocalStorageAPI localStorageAPI)
    {
        storageLocation = configuration["ChatSettings:ChatStorageLocation"];
        this.localStorageAPI = localStorageAPI;
    }

    private async Task<IQueryable<T>> GetContextAsync()
    {
        try
        {
            var context = await localStorageAPI.GetItemAsync<IEnumerable<T>>(storageLocation);

            if (context == null) return null!;

            IQueryable<T> query = context.AsQueryable();

            return query;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null)
    {
        var query = await GetContextAsync();

        if (query == null) return null;

        if (filter != null)
            query = query.Where(filter);

        return query.FirstOrDefault();
    }

    public async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
    {
        var query = await GetContextAsync();

        if (query == null) return null;

        if (filter != null)
            query = query.Where(filter);

        return query.ToList();
    }

    public async Task CreateAsync(ICollection<T> entity)
    {
        await SaveAsync(entity);
    }

    public async Task DeleteAsync(Expression<Func<T, bool>>? filter = null)
    {
        try
        {
            var query = await GetAllAsync();

            if (query == null || filter == null) return;

            var deleteEntity = query.FirstOrDefault(filter.Compile());

            query.Remove(deleteEntity);

            await SaveAsync(query);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not delete chat, Error: {ex}");
        }
    }

    public async Task SaveAsync(ICollection<T> entity)
    {
        await localStorageAPI.SetItemAsync(storageLocation, entity);
    }
}

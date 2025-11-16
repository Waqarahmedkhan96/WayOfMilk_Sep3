using System.Linq;
using Entities;
using RepositoryContracts;
using RepositoryContracts.ExceptionHandling;

namespace FileRepositories;

public class UserFileRepository : FileRepository<User>, IUserRepository
{
    public UserFileRepository() : base(Path.Combine("Data", "users.json")) { }

    public async Task<User> AddAsync(User user)
    {
        var items = await LoadAsync();

        if (string.IsNullOrWhiteSpace(user.UserName))
            throw new ValidationException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ValidationException("Password cannot be empty.");

        if (items.Any(u => u.UserName.Equals(user.UserName, StringComparison.OrdinalIgnoreCase)))
            throw new ValidationException($"Name '{user.UserName}' already exists.");
        // or: throw new DuplicateUserNameException(...);

        user.Id = items.Count == 0 ? 1 : items.Max(u => u.Id) + 1;

        items.Add(user);
        await SaveAsync(items);
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        var items = await LoadAsync();
        var idx = items.FindIndex(u => u.Id == user.Id);
        if (idx < 0)
            throw new NotFoundException($"User with ID {user.Id} not found.");

        if (string.IsNullOrWhiteSpace(user.UserName))
            throw new ValidationException("Name cannot be empty.");
        if (string.IsNullOrWhiteSpace(user.Password))
            throw new ValidationException("Password cannot be empty.");

        if (items.Any(u => u.Id != user.Id &&
                           u.UserName.Equals(user.UserName, StringComparison.OrdinalIgnoreCase)))
            throw new ValidationException($"Name '{user.UserName}' already exists.");

        items[idx] = user;
        await SaveAsync(items);
    }

    public async Task DeleteAsync(int id)
    {
        var items = await LoadAsync();
        var removed = items.RemoveAll(u => u.Id == id);
        if (removed == 0)
            throw new NotFoundException($"User with ID {id} not found.");
        await SaveAsync(items);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        var items = await LoadAsync();
        return items.SingleOrDefault(u => u.Id == id)
               ?? throw new NotFoundException($"User with ID {id} not found.");
    }

    public IQueryable<User> GetManyAsync()
        => LoadAsync().Result.AsQueryable(); // matches my interface
}


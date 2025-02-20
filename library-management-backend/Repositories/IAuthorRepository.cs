using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories;

public interface IAuthorRepository
{
    Task<Author> Add(Author author);

    Task<Author?> FindById(long authorId);

    Task<IList<Author>> FindAll(int pageNumber, int pageSize);

    Task Update(Author author);

    Task<Author> Remove(Author author);

    Task<int> CountAll();

    Task<bool> ExistsById(long authorId);
}

using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Repositories;

public interface IBookRepository
{
    Task<Book> Add(Book book);

    Task<Book?> FindById(long bookId);

    Task<ICollection<Book>> FindAll(int pageNumber, int pageSize);

    Task Update(Book book);

    Task<Book> Remove(Book book);

    Task<int> CountAll();

    Task<IList<Book>> FindAllByAuthorId(long authorId);

    Task<IList<Book>> Search(string query, int pageNumber, int pageSize);

    Task<int> CountAllByQuery(string searchQuery);
}

using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Extensions;

using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Repositories;

public class BookRepository(LibraryManagementSystemContext context) : IBookRepository
{
    private readonly LibraryManagementSystemContext _context = context;

    public async Task<Book> Add(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<ICollection<Book>> FindAll(int pageNumber, int pageSize)
    {
        return await _context.Books.ToListAsync();
    }

    public async Task<Book?> FindById(long bookId)
    {
        return await _context.Books.FirstOrDefaultAsync(book => book.Id == bookId);
    }

    public async Task Update(Book book)
    {
        _context.Books.Update(book);
        await _context.SaveChangesAsync();
    }

    public async Task<Book> Remove(Book book)
    {
        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<int> CountAll()
    {
        return await _context.Books.CountAsync();
    }

    public async Task<IList<Book>> FindAllByAuthorId(long authorId)
    {
        return await _context.Books.Where(book => book.AuthorId == authorId).ToListAsync();
    }

    public async Task<IList<Book>> Search(string query, int pageNumber, int pageSize)
    {
        return await _context.Books
            .FromSqlInterpolated($"SELECT * FROM {TABLE_NAME_BOOKS} AS b WHERE levenshtein(b.title::text, {query}::text) < {DEFAULT_LEVENSHTAIN_DISTANCE}")
            .ToListAsync();
    }

    public async Task<int> CountAllByQuery(string searchQuery)
    {
        return await _context.IntFromRawSqlAsync($"SELECT * FROM {TABLE_NAME_BOOKS} AS b WHERE levenshtein(b.title::text, @p0::text) < {DEFAULT_LEVENSHTAIN_DISTANCE}", searchQuery);
    }
}

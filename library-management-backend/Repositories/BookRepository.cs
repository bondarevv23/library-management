using LibraryManagementSystem.Data;
using LibraryManagementSystem.Extensions;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static LibraryManagementSystem.Constants.DatabaseConstants;

namespace LibraryManagementSystem.Repositories;

public class BookRepository(ApplicationSettings settings, LibraryManagementSystemContext context)
    : IBookRepository
{
    private readonly ApplicationSettings _settings = settings;
    private readonly LibraryManagementSystemContext _context = context;

    public async Task<Book> Add(Book book)
    {
        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<ICollection<Book>> FindAll(int pageNumber, int pageSize)
    {
        var offset = (pageNumber - 1) * pageSize;
        return await _context.Books.OrderBy(b => b.Id).Skip(offset).Take(pageSize).ToListAsync();
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
        var offset = (pageNumber - 1) * pageSize;
        string sql =
            $"SELECT * FROM {TABLE_NAME_BOOKS} AS b WHERE levenshtein(b.title::text, @p0::text) < {_settings.BookSearchMaxLevenshteinDistance} LIMIT {pageSize} OFFSET {offset}";
        return await _context.Books.FromSqlRaw(sql, query).ToListAsync();
    }

    public async Task<int> CountAllByQuery(string query)
    {
        string sql =
            $"SELECT COUNT(*) FROM {TABLE_NAME_BOOKS} AS b WHERE levenshtein(b.title::text, @p0::text) < {_settings.BookSearchMaxLevenshteinDistance}";
        return await _context.IntFromRawSqlAsync(sql, new NpgsqlParameter("@p0", query));
    }
}

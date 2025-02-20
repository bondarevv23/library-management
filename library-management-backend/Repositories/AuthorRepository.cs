using LibraryManagementSystem.Data;
using LibraryManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Repositories;

public class AuthorRepository(LibraryManagementSystemContext context) : IAuthorRepository
{
    private readonly LibraryManagementSystemContext _context = context;

    public async Task<Author> Add(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<IList<Author>> FindAll(int pageNumber, int pageSize)
    {
        return await _context.Authors
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Author?> FindById(long authorId)
    {
        return await _context.Authors.FirstOrDefaultAsync(author => author.Id == authorId);
    }

    public async Task Update(Author author)
    {
        _context.Authors.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task<Author> Remove(Author author)
    {
        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<int> CountAll()
    {
        return await _context.Authors.CountAsync();
    }

    public async Task<bool> ExistsById(long authorId)
    {
        return await _context.Authors.AnyAsync(author => author.Id == authorId);
    }
}

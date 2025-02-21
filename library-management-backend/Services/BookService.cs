using System.Web;
using AutoMapper;
using Fastenshtein;
using Microsoft.Extensions.Options;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.Controllers.Models;
using LibraryManagementSystem.Dto.Requests;
using LibraryManagementSystem.Dto.Responses;
using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;

using static LibraryManagementSystem.Constants.ValidationConstants;
using static LibraryManagementSystem.Constants.DatabaseConstants;
using static LibraryManagementSystem.Utilities.ServiceUtilities;

namespace LibraryManagementSystem.Services;

public class BookService(
    IOptions<ApplicationSettings> settings,
    IBookRepository repository,
    IAuthorRepository authorRepository,
    IValidationService validator,
    IMapper mapper
) : IBookService
{
    private readonly ApplicationSettings _settings = settings?.Value!;

    private readonly IBookRepository _repository = repository;

    private readonly IAuthorRepository _authorRepository = authorRepository;

    private readonly IValidationService _validator = validator;

    private readonly IMapper _mapper = mapper;

    public async Task<BookResponseDto> Create(CreateBookRequestModel request)
    {
        try
        {
            AssertValid(request);
            var authorId = request.Body!.AuthorId!.Value;
            await CheckAuthorExistsByIdAsync(authorId);
            var book = _mapper.Map<Book>(request.Body!);
            await _repository.Add(book);
            return _mapper.Map<BookResponseDto>(book);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<PagedResponseDto<BookResponseDto>> FindAll(FindAllBooksRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithPagableData(request);
            var pageNumber = request.PageNumber!.Value;
            var pageSize = request.PageSize!.Value;
            var totalCount = await _repository.CountAll();
            var books = await _repository.FindAll(pageNumber: pageNumber, pageSize: pageSize);
            var dtos = books.Select(_mapper.Map<BookResponseDto>).ToList();
            return new PagedResponseDto<BookResponseDto>(
                data: dtos,
                totalRecords: totalCount,
                pageSize: pageSize,
                pageNumber: pageNumber
            );
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<BookResponseDto> FindById(FindBookByIdRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithId(request);
            var bookId = request.Id!.Value;
            var book = await GetById(bookId);
            return _mapper.Map<BookResponseDto>(book);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task UpdateById(UpdateBookByIdRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithId(request);
            var bookId = request.Id!.Value;
            var book = await GetById(bookId);
            Update(book, request.Body!);
            await _repository.Update(book);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<BookResponseDto> DeleteById(DeleteBookByIdRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithId(request);
            var bookId = request.Id!.Value;
            var book = await GetById(bookId);
            await _repository.Remove(book);
            return _mapper.Map<BookResponseDto>(book);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<PagedResponseDto<BookResponseDto>> Search(SearchBooksRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithPagableData(request);
            var searchQuery = HttpUtility.UrlDecode(request.UrlEncodedSearchQuery!);
            var pageNumber = request.PageNumber!.Value;
            var pageSize = request.PageSize!.Value;
            return _settings.PerformFuzzySearchInMemory
                ? await FuzzySearchInMemory(searchQuery: searchQuery, pageNumber: pageNumber, pageSize: pageSize)
                : await FuzzySearchInDatabase(searchQuery: searchQuery, pageNumber: pageNumber, pageSize: pageSize);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }
    
    private async Task<PagedResponseDto<BookResponseDto>> FuzzySearchInMemory(
        string searchQuery,
        int pageNumber,
        int pageSize
    )
    {
        var levenshteinQuery = new Levenshtein(searchQuery);
        var takenBooks = new List<Book>(pageSize);
        var foundBooksCount = 0;
        var totalBooksCount = await _repository.CountAll();
        var fuzzySearchBucketSize = _settings.FuzzySearchBucketSize;
        var requestsCount = (totalBooksCount + fuzzySearchBucketSize - 1) / fuzzySearchBucketSize;
        for (int i = 0; i <= requestsCount; i++)
        {
            var bucketBooks = await _repository.FindAll(i, fuzzySearchBucketSize);
            foreach (var book in bucketBooks)
            {
                if (levenshteinQuery.DistanceFrom(book.Title) < _settings.BookSearchMaxLevenshteinDistance)
                {
                    foundBooksCount++;
                    if (pageNumber == 1)
                    {
                        takenBooks.Add(book);
                    }
                    if (foundBooksCount % pageSize == 0)
                    {
                        pageNumber--;
                    }
                }
            }
        }
        var dtos = takenBooks.Select(_mapper.Map<BookResponseDto>).ToList();
        return new PagedResponseDto<BookResponseDto>(
            data: dtos,
            totalRecords: foundBooksCount,
            pageSize: pageSize,
            pageNumber: pageNumber
        );
    }
    
    private async Task<PagedResponseDto<BookResponseDto>> FuzzySearchInDatabase(
        string searchQuery,
        int pageNumber,
        int pageSize
    )
    {
        var books = await _repository.Search(query: searchQuery, pageNumber: pageNumber, pageSize: pageSize);
        var totalRecordsCount = await _repository.CountAllByQuery(searchQuery);
        var dtos = books.Select(_mapper.Map<BookResponseDto>).ToList();
        return new PagedResponseDto<BookResponseDto>(
            data: dtos,
            totalRecords: dtos.Count,
            pageSize: pageSize,
            pageNumber: pageNumber
        );
    }

    private void AssertValid<T>(T value)
    {
        _validator.AssertValid<T>(value, INSTANCE_NAME_REQUEST_DATA);
    }

    private async Task CheckAuthorExistsByIdAsync(long authorId)
    {
        if (!await _authorRepository.ExistsById(authorId))
        {
            throw new NotFoundException(tableName: TABLE_NAME_AUTHORS, id: authorId);
        }
    }

    private async Task<Book> GetById(long id)
    {
        var book = await _repository.FindById(id);
        return book ?? throw new NotFoundException(tableName: TABLE_NAME_BOOKS, id: id);
    }

    private static void Update(Book book, UpdateBookRequestDto dto)
    {
        book.Title = dto.Title!;
        book.PublicationYear = dto.PublicationYear!.Value;
    }
}
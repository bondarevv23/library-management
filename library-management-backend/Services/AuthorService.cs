using AutoMapper;
using LibraryManagementSystem.Controllers.Models;
using LibraryManagementSystem.Dto.Requests;
using LibraryManagementSystem.Dto.Responses;
using LibraryManagementSystem.Exceptions;
using LibraryManagementSystem.Models;
using LibraryManagementSystem.Repositories;
using static LibraryManagementSystem.Constants.ValidationConstants;
using static LibraryManagementSystem.Constants.RedisConstants;
using static LibraryManagementSystem.Constants.DatabaseConstants;
using static LibraryManagementSystem.Utilities.ServiceUtilities;
using Newtonsoft.Json;

namespace LibraryManagementSystem.Services;

public class AuthorService(
    IAuthorRepository repository,
    IBookRepository bookRepository,
    IValidationService validator,
    IMapper mapper,
    ICacheService cache
) : IAuthorService
{
    private readonly IAuthorRepository _repository = repository;

    private readonly IBookRepository _bookRepository = bookRepository;

    private readonly IValidationService _validator = validator;

    private readonly IMapper _mapper = mapper;

    private readonly ICacheService _cache = cache;

    public async Task<AuthorResponseDto> Create(CreateAuthorRequestModel request)
    {
        try
        {
            AssertValid(request);
            await EvictCache();
            var author = _mapper.Map<Author>(request.Body!);
            await _repository.Add(author);
            return _mapper.Map<AuthorResponseDto>(author);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<PagedResponseDto<AuthorResponseDto>> FindAll(FindAllAuthorsRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithPagableData(request);
            var pageNumber = request.PageNumber!.Value;
            var pageSize = request.PageSize!.Value;
            var authorDtos = await GetAuthorsDtoFromCache(pageNumber: pageNumber, pageSize: pageSize) ??
                             await GetAuthorsDtoFromDatabaseAndSaveToCache(pageNumber: pageNumber, pageSize: pageSize);
            return authorDtos;
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<AuthorResponseDto> FindById(FindAuthorByIdRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithId(request);
            var authorId = request.Id!.Value;
            var author = await GetById(authorId);
            return _mapper.Map<AuthorResponseDto>(author);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task UpdateById(UpdateAuthorByIdRequestModel request)
    {
        try
        {
            AssertValid(request);
            await EvictCache();
            EnrichWithId(request);
            var authorId = request.Id!.Value;
            var author = await GetById(authorId);
            Update(author, request.Body!);
            await _repository.Update(author);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<AuthorResponseDto> DeleteById(DeleteAuthorByIdRequestModel request)
    {
        try
        {
            AssertValid(request);
            await EvictCache();
            EnrichWithId(request);
            var authorId = request.Id!.Value;
            var author = await GetById(authorId);
            await _repository.Remove(author);
            return _mapper.Map<AuthorResponseDto>(author);
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    public async Task<IList<BookResponseDto>> FindAllBooksById(FindAllBooksByAuthorIdRequestModel request)
    {
        try
        {
            AssertValid(request);
            EnrichWithId(request);
            var authorId = request.Id!.Value;
            await CheckExistsByIdAsync(authorId);
            var books = await _bookRepository.FindAllByAuthorId(authorId);
            return [.. books.Select(_mapper.Map<BookResponseDto>)];
        }
        catch (LibraryManagementSystemException exception)
        {
            EnrichErrorLogDescriptionWithOperationDetails(exception, request);
            throw;
        }
    }

    private async Task SaveToCache(PagedResponseDto<AuthorResponseDto> authorsDto)
    {
        var hashKey = BuildAuthorsHashKey(pageNumber: authorsDto.PageNumber, pageSize: authorsDto.PageSize);
        await _cache.Save(AUTHORS_REDIS_KEY, hashKey, authorsDto);
    }

    private async Task<PagedResponseDto<AuthorResponseDto>?> GetAuthorsDtoFromCache(int pageNumber, int pageSize)
    {
        var hashKey = BuildAuthorsHashKey(pageNumber: pageNumber, pageSize: pageSize);
        return await _cache.Get<PagedResponseDto<AuthorResponseDto>>(AUTHORS_REDIS_KEY, hashKey);
    }

    private static string BuildAuthorsHashKey(int pageNumber, int pageSize)
    {
        return string.Format(AUTHORS_HASH_KEY_TEMPLATE, pageNumber, pageSize);
    }

    private async Task<PagedResponseDto<AuthorResponseDto>> GetAuthorsDtoFromDatabaseAndSaveToCache(
        int pageNumber,
        int pageSize
    )
    {
        var totalCount = await _repository.CountAll();
        var authors = await _repository.FindAll(pageNumber: pageNumber, pageSize: pageSize);
        var authorDtos = authors.Select(_mapper.Map<AuthorResponseDto>).ToList();
        var pagedAuthors = new PagedResponseDto<AuthorResponseDto>(
            data: authorDtos,
            totalRecords: totalCount,
            pageSize: pageSize,
            pageNumber: pageNumber
        );
        await SaveToCache(pagedAuthors);
        return pagedAuthors;
    }

    private async Task EvictCache()
    {
        await _cache.DeleteByKey(AUTHORS_REDIS_KEY);
    }

    private void AssertValid<T>(T value)
    {
        _validator.AssertValid<T>(value, INSTANCE_NAME_REQUEST_DATA);
    }

    private async Task CheckExistsByIdAsync(long id)
    {
        if (!await _repository.ExistsById(id))
        {
            throw new NotFoundException(tableName: TABLE_NAME_AUTHORS, id: id);
        }
    }

    private async Task<Author> GetById(long id)
    {
        var author = await _repository.FindById(id);
        return author ?? throw new NotFoundException(tableName: TABLE_NAME_AUTHORS, id: id);
    }

    private static void Update(Author author, AuthorRequestDto dto)
    {
        author.Name = dto.Name!;
        author.DateOfBirth = DateOnly.Parse(dto.DateOfBirth!);
    }
}
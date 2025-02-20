using LibraryManagementSystem.Controllers.Models;
using LibraryManagementSystem.Dto;
using LibraryManagementSystem.Dto.Requests;
using LibraryManagementSystem.Dto.Responses;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;


[ApiController]
[Route("api/v1/books")]
public class BooksController(IBookService service) : ControllerBase
{
    private readonly IBookService _service = service;

    [HttpPost]
    public async Task<ActionResult<BookResponseDto>> Create([FromBody] BookRequestDto? body)
    {
        return Ok(await _service.Create(new CreateBookRequestModel { Body = body }));
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<BookResponseDto>>> FindAll(
        [FromQuery] string? pageNumber,
        [FromQuery] string? pageSize)
    {
        return Ok(await _service.FindAll(new FindAllBooksRequestModel
        {
            PageNumberPathVariable = pageNumber,
            PageSizePathVariable = pageSize
        }
        ));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookResponseDto>> FindById([FromRoute] string? id)
    {
        return Ok(await _service.FindById(new FindBookByIdRequestModel { IdPathVariable = id }));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateById([FromRoute] string? id, [FromBody] UpdateBookRequestDto body)
    {
        await _service.UpdateById(new UpdateBookByIdRequestModel { IdPathVariable = id, Body = body });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<BookResponseDto>> DeleteById([FromRoute] string? id)
    {
        return Ok(await _service.DeleteById(new DeleteBookByIdRequestModel { IdPathVariable = id }));
    }

    [HttpGet("search")]
    public async Task<ActionResult<PagedResponseDto<BookResponseDto>>> Search([FromQuery] string? query)
    {
        return Ok(await _service.Search(new SearchBooksRequestModel { UrlEncodedSearchQuery = query }));
    }
}

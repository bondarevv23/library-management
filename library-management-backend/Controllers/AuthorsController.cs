using LibraryManagementSystem.Controllers.Models;
using LibraryManagementSystem.Dto;
using LibraryManagementSystem.Dto.Requests;
using LibraryManagementSystem.Dto.Responses;
using LibraryManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.Controllers;

[ApiController]
[Route("api/v1/authors")]
public class AuthorsController(IAuthorService service) : ControllerBase
{
    private readonly IAuthorService _service = service;

    [HttpPost]
    public async Task<ActionResult<AuthorResponseDto>> Create([FromBody] AuthorRequestDto? body)
    {
        return Ok(await _service.Create(new CreateAuthorRequestModel { Body = body }));
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponseDto<AuthorResponseDto>>> FindAll(
        [FromQuery] string? pageNumber,
        [FromQuery] string? pageSize
    )
    {
        return Ok(
            await _service.FindAll(
                new FindAllAuthorsRequestModel
                {
                    PageNumberPathVariable = pageNumber,
                    PageSizePathVariable = pageSize,
                }
            )
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuthorResponseDto>> FindById([FromRoute] string? id)
    {
        return Ok(await _service.FindById(new FindAuthorByIdRequestModel { IdPathVariable = id }));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateById(
        [FromRoute] string? id,
        [FromBody] AuthorRequestDto body
    )
    {
        await _service.UpdateById(
            new UpdateAuthorByIdRequestModel { IdPathVariable = id, Body = body }
        );
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<AuthorResponseDto>> DeleteById([FromRoute] string? id)
    {
        return Ok(
            await _service.DeleteById(new DeleteAuthorByIdRequestModel { IdPathVariable = id })
        );
    }

    [HttpGet("{id}/books")]
    public async Task<ActionResult<IList<BookResponseDto>>> FindAllBooksById([FromRoute] string? id)
    {
        return Ok(
            await _service.FindAllBooksById(
                new FindAllBooksByAuthorIdRequestModel { IdPathVariable = id }
            )
        );
    }
}

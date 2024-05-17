using Application.Common.Dtos;
using Application.UseCases.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;
[Authorize]
public class CategoriesController : BaseApiController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetCategoryByIdQuery(this.UserId, id);
        return HandleResult(await this.Mediator.Send(query, cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery(this.UserId);
        return HandleResult(await this.Mediator.Send(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(
        [FromBody] TransactionCategoryDto category,
        CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(this.UserId, category);
        return HandleResult(await this.Mediator.Send(command, cancellationToken));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> EditCategory(
        Guid id,
        [FromBody] TransactionCategoryDto categoryDto,
        CancellationToken cancellationToken)
    {
        categoryDto = categoryDto with { Id = id };
        var command = new EditCategoryCommand(this.UserId, categoryDto);
        return HandleResult(await this.Mediator.Send(command, cancellationToken));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(this.UserId, id);
        return HandleResult(await this.Mediator.Send(command, cancellationToken));
    }
}
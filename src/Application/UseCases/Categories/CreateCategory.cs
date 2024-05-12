using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.ValueObjects;
using FluentValidation;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Categories;
public sealed record CreateCategoryCommand(TransactionCategoryDto Category) : ICommand<Unit>;

internal sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(cmd => cmd.Category.UserId)
            .NotEmpty()
            .WithMessage("User Id must not be empty.");
        
        RuleFor(cmd => cmd.Category.Name)
            .NotEmpty()
            .WithMessage("Name must not be empty.");
        
        RuleFor(cmd => cmd.Category.OperationType)
            .IsInEnum()
            .WithMessage("Operation Type must be a valid enum value.");
    }
}

internal sealed class CreateCategoryCommandHandler(ICategoryRepository categoryRepository) 
    : ICommandHandler<CreateCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<Result<Unit>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new TransactionCategory(
            request.Category.UserId,
            new Name(request.Category.Name),
            new Description(request.Category.Description), 
            (OperationType)request.Category.OperationType);

        await this._categoryRepository.Add(category);

        return Result.Success(Unit.Value);
    }
}
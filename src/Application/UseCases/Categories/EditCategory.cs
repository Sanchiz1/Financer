using Application.Abstractions.Messaging;
using Application.Common.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.AggregatesModel.TransactionAggregate.Repositories;
using Domain.Errors;
using FluentValidation;
using MediatR;
using SharedKernel.Result;

namespace Application.UseCases.Categories;
public sealed record EditCategoryCommand(string UserId, TransactionCategoryDto Category) : ICommand<Unit>;

internal class EditCategoryCommandValidator : AbstractValidator<EditCategoryCommand>
{
    public EditCategoryCommandValidator()
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

internal sealed class EditCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    ICategoryAuthorizationService categoryAuthorizationService,
    IMapper mapper)
    : ICommandHandler<EditCategoryCommand, Unit>
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly ICategoryAuthorizationService _categoryAuthorizationService = categoryAuthorizationService;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<Unit>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        bool isAccessible = await this._categoryAuthorizationService.IsCategoryAccessible(
           request.UserId,
           request.Category.Id,
           cancellationToken);

        if (!isAccessible)
        {
            return Result.Failure<Unit>(CategoryErrors.InvalidUser);
        }


        var category = await this._categoryRepository.GetByIdAsync(request.Category.Id, cancellationToken);

        if (category is null)
        {
            return Result.Failure<Unit>(CategoryErrors.NotFound);
        }

        category = this._mapper.Map<TransactionCategory>(request.Category);

        await this._categoryRepository.Update(category, cancellationToken);

        return Result.Success(Unit.Value);
    }
}
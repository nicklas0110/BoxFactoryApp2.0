using Application.DTOs;
using BoxFactoryApp;
using FluentValidation;

namespace Application.Validators;


public class PostBoxValidator : AbstractValidator<BoxDTOs>
{
    public PostBoxValidator()
    {
        RuleFor(p => p.Size).NotEmpty();
        RuleFor(p => p.Type).NotEmpty();
        RuleFor(p => p.CustomerName).NotEmpty();
    }
}
public class BoxValidator : AbstractValidator<Box>
{
    public BoxValidator()
    {
        RuleFor(p => p.size).NotEmpty();
        RuleFor(p => p.type).NotEmpty();
        RuleFor(p => p.customerName).NotEmpty();
        RuleFor(p => p.Id).GreaterThan(0);
    }
}
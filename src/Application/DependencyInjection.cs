using FluentValidation;
using System.Reflection;
using Application.Abstractions.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Domain.AggregatesModel.ReportAggregate;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddTransient<CurrencyConversionService>();
        services.AddTransient<ReportService>();

        return services;
    }
}
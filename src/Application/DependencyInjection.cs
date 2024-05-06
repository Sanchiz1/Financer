using FluentValidation;
using System.Reflection;
using Application.Abstractions.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using Domain.AggregatesModel.ReportAggregate;
using Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider;
using Domain.Yahoo;

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

        services.AddHttpClient<IYahooCurrencyAPI, YahooCurrencyAPI>();

        services.AddTransient<IExchangeRateProvider, ExchangeRateProviderProxy>();

        services.AddTransient<CurrencyConversionService>();

        return services;
    }
}
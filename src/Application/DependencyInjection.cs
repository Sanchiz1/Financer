using Application.Abstractions.Behaviors;
using Domain.AggregatesModel.ReportAggregate;
using Domain.AggregatesModel.ReportAggregate.ExchangeRateProvider;
using Domain.AggregatesModel.ReportAggregate.Reports.Builder;
using Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
using Domain.AggregatesModel.ReportAggregate.SaveReportStrategy;
using Domain.Yahoo;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

        services.AddTransient<IExpectsCurrency, ReportBuilder>();
        services.AddTransient<ICreateReportHandler, CreateMonthlyReportHandler>();

        services.AddKeyedTransient<IReportFileSaver, JsonReportFileSaver>("save-report-json");
        services.AddKeyedTransient<IReportFileSaver, XmlReportFileSaver>("save-report-xml");


        return services;
    }
}
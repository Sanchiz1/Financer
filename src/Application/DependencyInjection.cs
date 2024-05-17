using Application.Abstractions.Behaviors;
using Domain.AggregatesModel.ReportAggregate;
using Domain.AggregatesModel.ReportAggregate.CreateReportHandlers;
using Domain.AggregatesModel.ReportAggregate.CurrencyConversion;
using Domain.AggregatesModel.ReportAggregate.ReportBuilder;
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

        services.AddHttpClient<YahooCurrencyAPI>();
        services.AddTransient<IYahooCurrencyAPI, YahooCurrencyProxy>();

        services.AddTransient<IExchangeRateProvider, YahooExchangeRateAdapter>();

        services.AddTransient<CurrencyConversionService>();

        services.AddTransient<IExpectsCurrency, ReportBuilder>();

        services.AddReportHandlers();
        services.AddSingleton<CurrencyConversionService>();
        services.AddSingleton<ReportMakerFacade>();

        services.AddKeyedTransient<IReportFileSaver, JsonReportFileSaver>("save-report-json");
        services.AddKeyedTransient<IReportFileSaver, XmlReportFileSaver>("save-report-xml");

        return services;
    }

    public static void AddReportHandlers(this IServiceCollection services)
    {
        services.AddSingleton<CreateDailyReportHandler>();
        services.AddSingleton<CreateWeeklyReportHandler>();
        services.AddSingleton<CreateMonthlyReportHandler>();

        services.AddSingleton<ICreateReportHandler>(provider =>
        {
            var createDailyReportHandler = provider.GetRequiredService<CreateDailyReportHandler>();
            var createWeeklyReportHandler = provider.GetRequiredService<CreateWeeklyReportHandler>();
            var createMonthlyReportHandler = provider.GetRequiredService<CreateMonthlyReportHandler>();

            createMonthlyReportHandler
                .SetNext(createWeeklyReportHandler)
                .SetNext(createDailyReportHandler);

            return createMonthlyReportHandler;
        });
    }
}
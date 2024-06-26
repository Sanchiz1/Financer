﻿using Domain.AggregatesModel.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.ReportBuilder;
public interface IExpectsCurrency
{
    IExpectsSummary WithCurrency(Currency currency);
}

public interface IExpectsSummary
{
    IReportBuilder WithSummary(IEnumerable<Transaction> transactions);
    IReportBuilder WithMonthlySummary(IEnumerable<Transaction> transactions);
    IReportBuilder WithWeeklySummary(IEnumerable<Transaction> transactions);
    IReportBuilder WithDailySummary(IEnumerable<Transaction> transactions);
}

public interface IReportBuilder
{
    Report Build();
}

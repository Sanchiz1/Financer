﻿using Domain.AggregatesModel.ReportAggregate.Reports.Builder;
using Domain.Entities.TransactionAggregate;
using Domain.Extensions;

namespace Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
public class CreateMonthlyReportHandler : CreateReportHandler
{
    public IExpectsCurrency _reportBuilder;
    public CreateMonthlyReportHandler(IExpectsCurrency reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    public override Report CreateReport(IEnumerable<Transaction> transactions)
    {
        var dateRange = transactions.GetDateRange();
        if (dateRange.LengthInDays > 31)
        {
            var currency = base.GetCurrency(transactions);

            var report = _reportBuilder.WithCurrency(currency)
                .WithMonthlySummary(transactions)
                .Build();

            return report;
        }
        else
        {
            return base.CreateReport(transactions);
        }
    }
}
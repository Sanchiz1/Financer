﻿using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;

namespace Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
public abstract class CreateReportHandler : ICreateReportHandler
{
    private ICreateReportHandler _nextHandler;

    public ICreateReportHandler SetNext(ICreateReportHandler nextHandler)
    {
        _nextHandler = nextHandler;

        return nextHandler;
    }

    public virtual Report CreateReport(IEnumerable<Transaction> transactions)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.CreateReport(transactions);
        }
        else
        {
            throw new Exception();
        }
    }

    protected Currency GetCurrency(IEnumerable<Transaction> transactions)
    {
        var transaction = transactions.FirstOrDefault();

        return transaction is null ? Currency.None : transaction.Amount.Currency;
    }
}
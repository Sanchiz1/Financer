using Domain.AggregatesModel.ReportAggregate.Reports;
using Domain.AggregatesModel.ReportAggregate.Reports.Builder;
using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
public abstract class CreateReportHandler : ICreateReportHandler
{
    private ICreateReportHandler _nextHandler;

    public ICreateReportHandler SetNext(ICreateReportHandler nextHandler)
    {
        _nextHandler = nextHandler;

        return nextHandler;
    }

    public virtual Report CreateReport(IEnumerable<Transaction> transactions, Currency currency)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.CreateReport(transactions, currency);
        }
        else
        {
            throw new Exception();
        }
    }
}

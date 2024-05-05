using Domain.AggregatesModel.ReportAggregate.Reports;
using Domain.Entities.TransactionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ReportAggregate;
public abstract class CreateReportHandler : IReportService
{
    private CreateReportHandler _nextHandler;


    public CreateReportHandler SetNext(CreateReportHandler nextHandler)
    {
        this._nextHandler = nextHandler;

        return nextHandler;
    }

    public virtual Report CreateReport(IEnumerable<Transaction> transactions)
    {
        if (this._nextHandler != null)
        {
            return this._nextHandler.CreateReport(transactions);
        }
        else
        {
            throw new Exception();
        }
    }
}

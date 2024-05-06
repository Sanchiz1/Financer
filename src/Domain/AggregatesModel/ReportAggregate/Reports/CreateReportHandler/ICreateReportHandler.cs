using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
public interface ICreateReportHandler
{
    Report CreateReport(IEnumerable<Transaction> transactions, Currency currency);

    ICreateReportHandler SetNext(ICreateReportHandler handler);
}

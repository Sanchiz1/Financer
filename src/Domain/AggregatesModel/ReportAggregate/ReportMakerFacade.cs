using Domain.AggregatesModel.ReportAggregate.CurrencyConversion;
using Domain.AggregatesModel.ReportAggregate.Reports;
using Domain.AggregatesModel.ReportAggregate.Reports.CreateReportHandler;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.ValueObjects;
using SharedKernel.Result;

namespace Domain.AggregatesModel.ReportAggregate
{
    public sealed class ReportMakerFacade(CurrencyConversionService currencyConversionService, ICreateReportHandler createReportHandler)
    {
        private readonly CurrencyConversionService _currencyConversionService = currencyConversionService;
        private readonly ICreateReportHandler _createReportHandler = createReportHandler;

        public async Task<Result<Report>> CreateReport(Currency preferredCurrency, IEnumerable<Transaction> transactions, DateRange dateRange)
        {
            var convertedTransactions = await this._currencyConversionService.ConvertTransactionsAsync(transactions, preferredCurrency);

            return this._createReportHandler.CreateReport(convertedTransactions);
        }
    }
}
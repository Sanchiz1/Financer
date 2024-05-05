using Domain.Entities.TransactionAggregate;
using Domain.ValueObjects;
using SharedKernel.Result;

namespace Domain.AggregatesModel.ReportAggregate
{
    public sealed class ReportMakerFacade(CurrencyConversionService currencyConversionService, IReportService reportService)
    {
        private readonly CurrencyConversionService _currencyConversionService = currencyConversionService;
        private readonly IReportService _reportService = reportService;

        public async Task<Result<Report>> CreateReport(Currency preferredCurrency, IEnumerable<Transaction> transactions, DateRange dateRange)
        {
            var convertedTransactions = await this._currencyConversionService.ConvertTransactionsAsync(transactions, preferredCurrency);

            return this._reportService.CreateReport(convertedTransactions);
        }
    }
}

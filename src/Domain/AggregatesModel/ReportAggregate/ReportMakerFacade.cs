using Domain.AggregatesModel.ReportAggregate.CreateReportHandlers;
using Domain.AggregatesModel.ReportAggregate.CurrencyConversion;
using Domain.AggregatesModel.TransactionAggregate;
using Domain.ValueObjects;
using SharedKernel.Result;

namespace Domain.AggregatesModel.ReportAggregate
{
    public sealed class ReportMakerFacade
    {
        private static Error UnableToCreate = new("Report.UnableToCreate", "Unable to create report");

        private readonly CurrencyConversionService _currencyConversionService;
        private readonly ICreateReportHandler _createReportHandler;

        public ReportMakerFacade(
            CurrencyConversionService currencyConversionService, 
            ICreateReportHandler createReportHandler)
        {
            this._currencyConversionService = currencyConversionService;
            this._createReportHandler = createReportHandler;
        }

        public async Task<Result<Report>> CreateReport(Currency preferredCurrency, IEnumerable<Transaction> transactions)
        {
            if (!transactions.Any())
            {
                return Result.Failure<Report>(UnableToCreate);
            }

            var convertedTransactions = await this._currencyConversionService.ConvertTransactionsAsync(transactions, preferredCurrency);

            var report = this._createReportHandler.CreateReport(convertedTransactions);

            return report;
        }
    }
}
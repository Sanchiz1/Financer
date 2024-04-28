using Domain.Users;
using Domain.Funds;
using Domain.Currencies;
using Domain.Categories;
using Domain.Transactions;
using Domain.Abstractions;

namespace Domain.Reports
{
    public class ReportService(
        CurrencyConversionService currencyConversionService,
        ITransactionRepository transactionRepository,
        IFundRepository fundRepository)
    {
        private readonly CurrencyConversionService _currencyConversionService = currencyConversionService;
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IFundRepository _fundRepository = fundRepository;

        public async Task<Result<Report>> CreateFundReport(IUser user, DateRange dateRange, Guid fundId, ICategoryRepository categoryRepository, CancellationToken cancellationToken = default)
        {
            IEnumerable<Transaction> transactions = await this._transactionRepository.GetUserTransactionsOfFundAsync(user, dateRange, fundId, cancellationToken);

            Fund? fund = await this._fundRepository.GetByIdAsync(fundId, cancellationToken);
            
            if (fund is null)
            {
                return Result.Failure<Report>(FundErrors.NotFound);
            }

            Currency prefferedCurrency = fund.Currency;

            IEnumerable<Transaction> convertedTransactions = this._currencyConversionService.ConvertTransactions(transactions, prefferedCurrency);

            return await Report.Create(convertedTransactions, dateRange, prefferedCurrency, categoryRepository);
        }

        public async Task<Result<Report>> CreateUserTransactionsReport(IUser user, DateRange dateRange, ICategoryRepository categoryRepository, CancellationToken cancellationToken = default)
        {
            IEnumerable<Transaction> transactions = await this._transactionRepository.GetUserTransactionsAsync(user, dateRange, cancellationToken);

            Currency prefferedCurrencty = user.PrefferedCurrency;

            IEnumerable<Transaction> convertedTransactions = this._currencyConversionService.ConvertTransactions(transactions, prefferedCurrencty);

            return await Report.Create(convertedTransactions, dateRange, prefferedCurrencty, categoryRepository);
        }
    }
}
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
        IFundRepository fundRepository,
        ICurrencyRepository currencyRepository)
    {
        private readonly CurrencyConversionService _currencyConversionService = currencyConversionService;
        private readonly ITransactionRepository _transactionRepository = transactionRepository;
        private readonly IFundRepository _fundRepository = fundRepository;
        private readonly ICurrencyRepository _currencyRepository = currencyRepository;
        
        public async Task<Result<Report>> CreateFundReport(User user, DateRange dateRange, Guid fundId, ICategoryRepository categoryRepository, CancellationToken cancellationToken = default)
        {
            IEnumerable<Transaction> transactions = await this._transactionRepository.GetUserTransactionsOfFundAsync(user, dateRange, fundId, cancellationToken);

            Currency prefferedCurrency = await this._fundRepository.GetFundCurrency(fundId, cancellationToken);

            IEnumerable<Transaction> convertedTransactions = this._currencyConversionService.ConvertTransactions(transactions, prefferedCurrency);

            return await Report.Create(convertedTransactions, dateRange, prefferedCurrency, categoryRepository);
        }

        public async Task<Result<Report>> CreateUserTransactionsReport(User user, DateRange dateRange, ICategoryRepository categoryRepository, CancellationToken cancellationToken = default)
        {
            IEnumerable<Transaction> transactions = await this._transactionRepository.GetUserTransactionsAsync(user, dateRange, cancellationToken);
            
            Currency? prefferedCurrencty = await this._currencyRepository.GetByIdAsync(user.PreferredCurrencyId, cancellationToken);
            
            if (prefferedCurrencty is null)
            {
                return Result.Failure<Report>(CurrencyErrors.NotFound);
            }

            IEnumerable<Transaction> convertedTransactions = this._currencyConversionService.ConvertTransactions(transactions, prefferedCurrencty);

            return await Report.Create(convertedTransactions, dateRange, prefferedCurrencty, categoryRepository);
        }
    }
}
# Oleksandr Zaitsev:

## Patterns

### 1. [Builder](https://github.com/Sanchiz1/Financer/tree/main/src/Domain/AggregatesModel/ReportAggregate/ReportBuilder) 

Builder Pattern was implemented for creating Reports with Summaries. It has methods for setting currency and for creating summaries for different time periods.

``` c#
public IExpectsSummary WithCurrency(Currency currency)
{
    Currency = currency;

    return this;
}

public IReportBuilder WithMonthlySummary(IEnumerable<Transaction> transactions)
{
    SummaryMapper summaryMapper = new MonthlySummaryMapper();

    Summaries = summaryMapper.MapToSummaries(transactions).ToList();

    return this;
}

public Report Build()
{
    var report = new Report(Currency, Summaries);

    Currency = Currency.None;
    Summaries = [];

    return report;
}

```


### 2. [Chain Of Responsibility](https://github.com/Sanchiz1/Financer/tree/main/src/Domain/AggregatesModel/ReportAggregate/CreateReportHandlers)

CoR pattern was implemented for handling variations of report depending on transactions date range. It uses builder for creating report with monthly summaries, if date range more than month, weekly - if more than week or daily.

``` c#
public override Report CreateReport(IEnumerable<Transaction> transactions)
{
    var dateRange = transactions.GetDateRange();
    if (dateRange.LengthInDays > 6)
    {
        var currency = GetCurrency(transactions);

        var report = _reportBuilder.WithCurrency(currency)
            .WithWeeklySummary(transactions)
            .Build();

        return report;
    }
    else
    {
        return base.CreateReport(transactions);
    }
}
```

``` c#
createMonthlyReportHandler
    .SetNext(createWeeklyReportHandler)
    .SetNext(createDailyReportHandler);

```

### 3. [Adapter](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/YahooExchangeRateAdapter.cs)

Adapter pattern was needed, because application has *IYahooCurrencyAPI* for fetching exchange rates, but rest of services uses *IExchangeProvider*, so adapter helps using YahooApi as *IExchangeProvider*.

``` c#
public class YahooExchangeRateAdapter : IExchangeRateProvider
{
    private readonly IYahooCurrencyAPI _yahooCurrencyAPI;

    public YahooExchangeRateAdapter(IYahooCurrencyAPI yahooCurrencyAPI)
    {
        _yahooCurrencyAPI = yahooCurrencyAPI;
    }

    public async Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency)
    {
        return await _yahooCurrencyAPI.GetExchangeRateAsync(fromCurrency.Code, toCurrency.Code);
    }
}
```

### 4. [Template Method](https://github.com/Sanchiz1/Financer/tree/main/src/Domain/AggregatesModel/ReportAggregate/ReportBuilder/SummaryMappers)

Report Builder had very similar methods for creating summaries, so Template Method was introduced. SummaryMapper class was created, it has basic algorithm for creating summaries and set of methods for changing parts of this algorithm.

``` c#
public abstract class SummaryMapper
{
    public IEnumerable<Summary> MapToSummaries(IEnumerable<Transaction> transactions)
    {
        var summaries = new List<Summary>();

        var dateRange = transactions.GetDateRange();

        var currentDate = GetStartDate(dateRange.Start);

        while (currentDate <= dateRange.End)
        {
            decimal totalAmount = GetTotalAmount(currentDate, transactions);

            var summary = CreateSummary(currentDate, totalAmount);

            summaries.Add(summary);

            currentDate = NextDate(currentDate);
        }

        return summaries;
    }

    protected abstract DateOnly GetStartDate(DateOnly date);
    protected abstract DateOnly NextDate(DateOnly currentDate);
    protected abstract decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions);
    protected abstract Summary CreateSummary(DateOnly currentDate, decimal amount);
}
```

``` c#
public class DailySummaryMapper : SummaryMapper
{
    protected override DateOnly GetStartDate(DateOnly date)
    {
        return date;
    }
    protected override DateOnly NextDate(DateOnly currentDate)
    {
        return currentDate.AddDays(1);
    }

    protected override decimal GetTotalAmount(DateOnly currentDate, IEnumerable<Transaction> transactions)
    {
        return transactions.Where(t =>
        t.OperationDate.IsDate(currentDate))
            .Sum(o => o.RealAmount);
    }

    protected override Summary CreateSummary(DateOnly currentDate, decimal amount)
    {
        return new Summary(amount,
            DateRange.Create(currentDate, currentDate)
            );
    }
}

```


## Principles


## Refactoring Techniques



# Illia Kotvitskyi

## Patterns

### 1. Strategy

### 2. Proxy

### 3. Facade


## Principles


## Refactoring Techniques

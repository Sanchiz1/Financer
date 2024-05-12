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

### 1. DRY

Code was written by DRY principle, for example applicaiton has plenty of extension methods for working with dates.

``` c#
public static class DateExtensions
{
    public static DateOnly GetStartOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    public static DateOnly GetEndOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static DateOnly GetStartOfWeek(this DateOnly date)
    {
        int delta = DayOfWeek.Monday - date.DayOfWeek;
        return date.AddDays(delta);
    }

    public static DateOnly GetEndOfWeek(this DateOnly date)
    {
        return date.GetStartOfWeek().AddDays(6);
    }
    
    public static bool IsDate(this DateTime date, DateOnly otherDate)
    {
        return date.Year == otherDate.Year &&
            date.Month == otherDate.Month &&
            date.Day == otherDate.Day;
    }
}
```

### 2. DIP

Dependency inversion principle was used, classes depend on interface **IExchangeRateProvider** - higher level module, instead of a concrete class. 

``` c#
public interface IExchangeRateProvider
{
    Task<decimal> GetExchangeRateAsync(Currency fromCurrency, Currency toCurrency);
}

```

### 3. SRP

SRP was used, because every class implements its own logic and responsible for small chunk of functionality. For example ReportBuilder was created to build reports so CreateReportHendlers would be responsible only for deciding, how to divide summaries and not some other functionality.

### 4. Fail Fast

After fetching rate, *YahooCurrencyApi* checks status code of response and throws error if it is not success:

``` c#
public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
{
    HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

    if (!response.IsSuccessStatusCode) throw new YahooException("Failed to fetch data");

    string jsonString = await response.Content.ReadAsStringAsync();

    var node = JsonNode.Parse(jsonString)!;

    var rate = node["chart"]?["result"]?[0]?["meta"]?["regularMarketPrice"]?.ToString();

    if (rate is null)
    {
        throw new YahooException("Failed to parse data");
    }

    return decimal.Parse(rate);
}
```
### 5. YAGNI

Only required functionality was implemented, application remains structure of actually needed modules with everything having a purpose.

## Refactoring Techniques

### 1. Form Template Method

Builder methods for creating summaries had very similar code. In [commit #8ae81b8](https://github.com/Sanchiz1/Financer/commit/8ae81b86b162e87562afc17d7d8ebdaf0bae98fc) template method was formed to remove duplication. 
Template method was described earlier.

### 2. Intoduce foreign method

Foreign method was introduced for getting date range the list of transactions.

``` c#
public static DateRange GetDateRange(this IEnumerable<Transaction> transactions)
{
    var minDate = transactions.Min(t => t.OperationDate);
    var maxDate = transactions.Max(t => t.OperationDate);

    return DateRange.Create(
        DateOnly.FromDateTime(minDate),
        DateOnly.FromDateTime(maxDate));
}
```

### 3. Extract Method

Writing *IdentityService* code for mapping *IdentityResult* was moved to new separate method.

``` c#
public async Task<Result> DeleteUserAsync(Guid userId)
{
    var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

    if (user is null) return Result.Failure<string>(IdentityErrors.UserNotFound);

    var result = await _userManager.DeleteAsync(user);

    return MapIdentityResult(result);
}

private static Result MapIdentityResult(IdentityResult result)
{
    if (!result.Succeeded)
    {
        return Result.Failure(result.Errors.First().ToResultError());
    }

    return Result.Success();
}
```

# Illia Kotvitskyi

## Patterns

### 1. Strategy

### 2. Proxy

### 3. Facade


## Principles


## Refactoring Techniques

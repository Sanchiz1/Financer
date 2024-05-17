# Financer

## Description
The Financer application is a Web API developed using ASP .NET Core framework. Its primary functionality enables users to meticulously monitor both their expenditures and incomes, facilitating the generation of reports tailored to individual financial activities.


## How to run ?
To run the Financer project, follow these steps:

Ensure that you have Microsoft SQL Server installed on your system. If not, download and install it from the official Microsoft website.

Clone the Financer repository to your machine.

Open the solution file in Visual Studio or your preferred integrated development environment (IDE).

Navigate to the appsettings.Development.json file in the `Web` project. Update the connection string under the "ConnectionStrings" section to point to your MS SQL Server instance.

Run the app to create database automatically.

Once the database migration and seeding processes are complete, build the solution to ensure all dependencies are resolved.

Run the application. Ensure that the API endpoints are accessible and that you can interact with the application without encountering any errors.

# Oleksandr Zaitsev

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
Used to save reports to a file in JSON or XML formats.

### Implementation

#### Interface and Concrete Implementations
- [**IReportFileSaver**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/SaveReportStrategy/IReportFileSaver.cs): Defines an interface for the report file saving strategy. It declares a method `SaveReport` to save a report.
   ```csharp
   public interface IReportFileSaver
   {
      byte[] SaveReport(Report report);
   }
   ```

- [**JsonReportFileSaver**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/SaveReportStrategy/JsonReportFileSaver.cs): Implements `IReportFileSaver` for saving reports in JSON format.
   ```csharp
   public sealed class JsonReportFileSaver : IReportFileSaver
   {
      public byte[] SaveReport(Report report)
      {
         // Implementation of JSON serialization logic here
      }
   }
   ```
- [**XmlReportFileSaver**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/SaveReportStrategy/XmlReportFileSaver.cs): Implements `IReportFileSaver` for saving reports in XML format.
   ```csharp
   public sealed class XmlReportFileSaver : IReportFileSaver
   {
      public byte[] SaveReport(Report report)
      {
         // Implementation of XML serialization logic here
      }
   }
   ```
#### Service Registration
The `IReportFileSaver` implementations are registered in the DI container with unique keys (`"save-report-json"` and `"save-report-xml"`).

   ```csharp
   // Registration of services in DI container
   services.AddKeyedTransient<IReportFileSaver, JsonReportFileSaver>("save-report-json");
   services.AddKeyedTransient<IReportFileSaver, XmlReportFileSaver>("save-report-xml");
   ```

#### Controller Methods
- **SaveReportAsJson**: Controller method for saving reports as JSON. It uses a corresponding query handler to handle the saving logic.
- **SaveReportAsXml**: Controller method for saving reports as XML. Similar to the JSON method, it uses a query handler for XML saving.

#### Query Handlers
- **SaveReportJsonQueryHandler**: Handles the saving of reports as JSON. It uses the `JsonReportFileSaver` implementation to save the report.
- **SaveReportXmlQueryHandler**: Handles the saving of reports as XML. It utilizes the `XmlReportFileSaver` implementation for XML saving.

   ```csharp
   // Handler for saving report as JSON
   internal sealed class SaveReportJsonQueryHandler : IQueryHandler<SaveReportJsonQuery, ReportFile>
   {
      private readonly IReportFileSaver _jsonSaver;

      public SaveReportJsonQueryHandler([FromKeyedServices("save-report-json")] IReportFileSaver jsonSaver)
      {
         _jsonSaver = jsonSaver;
      }

      public async Task<Result<ReportFile>> Handle(SaveReportJsonQuery request, CancellationToken cancellationToken)
      {
         // Logic for generating report and saving it as JSON using _jsonSaver
      }
   }

   // Handler for saving report as XML
   internal sealed class SaveReportXmlQueryHandler : IQueryHandler<SaveReportXmlQuery, ReportFile>
   {
      private readonly IReportFileSaver _xmlSaver;

      public SaveReportXmlQueryHandler([FromKeyedServices("save-report-xml")] IReportFileSaver xmlSaver)
      {
         _xmlSaver = xmlSaver;
      }

      public async Task<Result<ReportFile>> Handle(SaveReportXmlQuery request, CancellationToken cancellationToken)
      {
         // Logic for generating report and saving it as XML using _xmlSaver
      }
   }
   ```

This approach allows for easy extension and modification of report storage formats in the future, adhering to the **open/closed principle** of software design.

### 2. Proxy

The Proxy pattern is utilized in our project to control access to the Yahoo currency API for fetching exchange rates. It allows for additional functionalities, such as caching the retrieved exchange rates to improve performance and reduce unnecessary API calls.

### Implementation
   ```csharp
   public interface IYahooCurrencyAPI
   {
      Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode);
   }
   
   public sealed class YahooCurrencyProxy(YahooCurrencyAPI currencyAPI) : IYahooCurrencyAPI
    {
        private readonly YahooCurrencyAPI _currencyAPI = currencyAPI;
        private readonly ConcurrentDictionary<(string, string), decimal> _cachedRates = [];

        public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
        {
            if (_cachedRates.TryGetValue((fromCurrencyCode, toCurrencyCode), out var rate))
            {
                return rate;
            }
            else
            {
                decimal currencyRate = await _currencyAPI.GetExchangeRateAsync(fromCurrencyCode, toCurrencyCode);

                this._cachedRates.AddOrUpdate((fromCurrencyCode, toCurrencyCode), currencyRate, (key, existingValue) => currencyRate);

                return currencyRate;
            }
        }
    }
   ```

#### Proxy Class
- [**YahooCurrencyProxy**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/Yahoo/YahooCurrencyProxy.cs): Acts as a proxy for the Yahoo currency API. Implements the `IYahooCurrencyAPI` interface and internally manages the interaction with the actual Yahoo currency API [`IYahooCurrencyAPI`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/Yahoo/IYahooCurrencyAPI.cs).

The [`CurrencyConversionService`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/CurrencyConversionService.cs) utilizes the [`IExchangeRateProvider`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/IExchangeRateProvider.cs) interface, which can be a direct implementation or a proxy to fetch exchange rates from the Yahoo currency API. The proxy ensures efficient retrieval of exchange rates by caching them and minimizing unnecessary API calls.

This approach enhances performance and reliability by abstracting the complexity of exchange rate retrieval and caching, providing a seamless experience for currency conversion operations.

### 3. Facade
### Purpose
The Facade pattern simplifies the interface to a complex system or set of subsystems by providing a unified interface. In our project, the Facade is utilized to streamline the process of report generation, which involves currency conversion and report creation from a list of transactions.

### Implementation

   ```csharp
   public sealed class ReportMakerFacade(CurrencyConversionService currencyConversionService, ICreateReportHandler createReportHandler)
   {
      private readonly CurrencyConversionService _currencyConversionService = currencyConversionService;
      private readonly ICreateReportHandler _createReportHandler = createReportHandler;

      public async Task<Report> CreateReport(Currency preferredCurrency, IEnumerable<Transaction> transactions)
      {
         var convertedTransactions = await this._currencyConversionService.ConvertTransactionsAsync(transactions, preferredCurrency);

         return this._createReportHandler.CreateReport(convertedTransactions);
      }
   }
   ```

#### Facade Class
- [**ReportMakerFacade**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/ReportMakerFacade.cs): Acts as a facade to simplify the process of report generation. It abstracts away the complexities of currency conversion and report creation by delegating these tasks to the [`CurrencyConversionService`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/CurrencyConversionService.cs) and [`ICreateReportHandler`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CreateReportHandlers/ICreateReportHandler.cs) implementation, respectively.

## Principles

### Fail Fast Principle

The Fail Fast principle suggests that a system should immediately halt execution upon encountering an error or inconsistency, rather than attempting to continue with potentially corrupted data. In the provided code for the [`Money`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/ValueObjects/Money.cs) class, this principle is implemented through the following mechanisms:

1. **Currency Equality Check**: 
   - Before performing arithmetic operations like addition, subtraction, multiplication, and division, the `Money` class checks if the currencies of the operands are equal.
   - If the currencies are not equal, it throws an `InvalidOperationException` with an appropriate error message, halting further execution.

      ```csharp
      public static Money operator +(Money left, Money right)
      {
         if (left.Currency != right.Currency)
         {
            throw new InvalidOperationException("Currencies have to be equal.");
         }

         return new Money(left.Amount + right.Amount, left.Currency);
      }
      ```

2. **Comparison Operations**:
   - Similar to arithmetic operations, comparison operators such as greater than, less than, greater than or equal to, and less than or equal to are overridden.
   - Before comparing two `Money` objects, the class verifies if their currencies are equal.
   - If the currencies are not equal, it raises an `ArgumentException` with a descriptive error message, adhering to the Fail Fast principle by stopping further processing.

      ```csharp
      public static bool operator >(Money left, Money right) =>
         left.Currency == right.Currency ? left.Amount > right.Amount
         : RaiseCurrencyComparisonError(left, right);

      public static bool operator <(Money left, Money right) =>
         left.Currency == right.Currency ? left.Amount < right.Amount
         : RaiseCurrencyComparisonError(left, right);

      public static bool operator >=(Money left, Money right) =>
         left.Currency == right.Currency ? left.Amount >= right.Amount
         : RaiseCurrencyComparisonError(left, right);

      public static bool operator <=(Money left, Money right) =>
         left.Currency == right.Currency ? left.Amount <= right.Amount
         : RaiseCurrencyComparisonError(left, right);

      private static bool RaiseCurrencyComparisonError(Money a, Money b) =>
         RaiseCurrencyError<bool>("compare", a, b);

      private static T RaiseCurrencyError<T>(string operation, Money a, Money b) =>
         throw new ArgumentException($"Cannot {operation} {a.Currency} and {b.Currency}");
      ```

By incorporating these checks and validations, the `Money` class ensures that any potential errors related to currency discrepancies are detected and addressed immediately, adhering to the Fail Fast principle.

### KISS Principle

The KISS (Keep It Simple, Stupid) principle advocates for simplicity in design and implementation. The [`Currency`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/ValueObjects/Currency.cs) class exemplifies this principle through its straightforward structure and functionality:

   ```csharp
   public class Currency : ValueObject
   {
      public string Code { get; init; }

      private Currency() { this.Code = string.Empty; }
      private Currency(string code) => this.Code = code;

      internal static readonly Currency None = new(string.Empty);
      public static readonly Currency Usd = new("USD");
      public static readonly Currency Eur = new("EUR");
      public static readonly Currency Uah = new("UAH");

      public static Currency FromCode(string code)
      {
         return All.FirstOrDefault(c => c.Code == code) ??
               throw new ApplicationException("The currency is invalid.");
      }

      [JsonIgnore]
      public Money MinPositiveValue =>
         new(.01M, this);

      public Money Of(decimal amount) =>
         new(amount, this);

      public override string ToString() =>
         this.Code;

      protected override IEnumerable<object> GetEqualityComponents()
      {
         yield return this.Code;
      }

      public static readonly IReadOnlyCollection<Currency> All = [Usd, Eur, Uah];
   }
   ```

1. **Minimalistic Design**:
   - The class has a simple structure, consisting of a single property (`Code`), a constructor, and a few static fields for common currency codes (`Usd`, `Eur`, `Uah`), along with a private constructor to ensure controlled instantiation.
   - By avoiding unnecessary complexity and keeping the class concise, it adheres to the KISS principle.

2. **Clear and Direct Methods**:
   - The `FromCode` method provides a clear and direct way to create `Currency` objects based on their currency codes.
   - The `Of` method allows for creating `Money` objects with the specified amount and the current currency instance.
   - Both methods have straightforward implementations, enhancing readability and maintainability.

3. **Limited Responsibility**:
   - The `Currency` class focuses solely on representing currency information, without incorporating additional functionalities unrelated to its purpose.
   - This limited responsibility ensures that the class remains focused and easy to understand, aligning with the KISS principle.

4. **Immutable State**:
   - The `Code` property is initialized only once and cannot be modified after instantiation, promoting immutability and reducing the risk of unintended changes.
   - This immutable state simplifies the class's behavior and enhances predictability, in line with the KISS principle.

By adhering to a simple and straightforward design, the `Currency` class embodies the KISS principle, making it easier to comprehend, use, and maintain.

### Composition Over Inheritance Principle

The Composition Over Inheritance principle advocates favoring composition (object composition) over inheritance (class inheritance) to achieve code reuse and flexibility. The [`YahooCurrencyProxy`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/Yahoo/YahooCurrencyProxy.cs) class exemplifies this principle through its design and usage of composition:

   ```csharp
   public sealed class YahooCurrencyProxy(YahooCurrencyAPI currencyAPI) : IYahooCurrencyAPI
   {
      private readonly YahooCurrencyAPI _currencyAPI = currencyAPI;
      private readonly ConcurrentDictionary<(string, string), decimal> _cachedRates = [];

      public async Task<decimal> GetExchangeRateAsync(string fromCurrencyCode, string toCurrencyCode)
      {
         // Implementation details
      }
   }
   ```

1. **Composition-based Design**:
   - Instead of inheriting behavior from a base class, the `YahooCurrencyProxy` class utilizes composition by containing an instance of the `YahooCurrencyAPI` class (`_currencyAPI`).
   - Through composition, the `YahooCurrencyProxy` class can leverage the functionality of the `YahooCurrencyAPI` class without inheriting its implementation details, promoting code reuse and flexibility.

2. **Encapsulation of Functionality**:
   - The `YahooCurrencyProxy` class encapsulates the functionality of retrieving exchange rates from the YahooCurrencyAPI within its own implementation.
   - By encapsulating this functionality, the class maintains a clear and focused purpose, avoiding potential complexities associated with inheritance hierarchies.

3. **Flexibility and Extensibility**:
   - Composition allows the `YahooCurrencyProxy` class to easily adapt to changes in the behavior or implementation of the `YahooCurrencyAPI` class.
   - It enables flexibility in switching or substituting the underlying implementation (`YahooCurrencyAPI`) without affecting the overall functionality of the `YahooCurrencyProxy` class.

4. **Single Responsibility Principle (SRP)**:
   - The `YahooCurrencyProxy` class adheres to the SRP by focusing solely on the responsibility of acting as a proxy for accessing exchange rates from the `YahooCurrencyAPI`.
   - This clear delineation of responsibility enhances maintainability and readability, contributing to a well-structured codebase.

Through composition, the `YahooCurrencyProxy` class promotes code reuse, flexibility, encapsulation, and adherence to the SRP, aligning with the Composition Over Inheritance principle.


### Single Responsibility Principle (SRP)

The [`BaseApiController`](https://github.com/Sanchiz1/Financer/blob/main/src/Web/Controllers/BaseApiController.cs) class follows the Single Responsibility Principle by focusing solely on handling HTTP requests and responses for API controllers. It defines the structure and behavior of API endpoints through route attributes and provides methods for consistent handling of HTTP responses based on operation results. Segregating these responsibilities into a base controller class enhances maintainability and extensibility.

   ```csharp
   public class BaseApiController : ControllerBase
   {
      private IMediator _mediator;

      protected IMediator Mediator => this._mediator ??= this.HttpContext.RequestServices.GetService<IMediator>()!;

      protected string UserId => this.User.GetUserId().ToString();

      protected ActionResult HandleResult<T>(Result<T> result)
      {
         if (result is null)
         {
               return NotFound();
         }

         if (result.IsSuccess && result.Value != null)
         {
               return Ok(result.Value);
         }

         if (result.IsSuccess && result.Value == null)
         {
               return NotFound();
         }

         return BadRequest(result.Error);
      }

      protected ActionResult HandleResult(Result result)
      {
         if (result is null)
         {
               return NotFound();
         }

         if (result.IsSuccess)
         {
               return Ok();
         }

         return BadRequest(result.Error);
      }
   }
   ```


### Dependency Inversion Principle

The Dependency Inversion Principle states that high-level modules should not depend on low-level modules, but both should depend on abstractions. In the `SaveReportXmlQueryHandler` class, this principle is adhered to through the use of interfaces for dependencies.

1. **Interface for `ITransactionRepository`**: The class accepts `ITransactionRepository` as a parameter in its constructor, allowing it to use any implementation of this interface, providing flexibility and the ability to replace dependencies.

2. **Interface for `IReportFileSaver`**: Dependency on `IReportFileSaver` also indicates compliance with this principle. The class depends on an abstraction rather than a specific implementation.

3. **Interface for `ReportMakerFacade`**: Similarly, dependency on `ReportMakerFacade` is through its abstraction, adhering to the Dependency Inversion Principle.

This approach makes it easy to change the implementations of these dependencies without modifying the [`SaveReportXmlQueryHandler`](https://github.com/Sanchiz1/Financer/blob/main/src/Application/UseCases/Reports/SaveReportXml.cs) class, supporting flexibility and ease of making changes.


## Refactoring Techniques

### Extract Method

This technique involves isolating a segment of code into a separate method to improve readability, maintainability, and reusability.

   ```csharp
   [HttpPost("save/json")]
   public async Task<IActionResult> SaveReportAsJson(
      string currencyCode,
      DateOnly startDate,
      DateOnly endDate,
      CancellationToken cancellationToken)
   {
      var query = new SaveReportJsonQuery(Currency.FromCode(currencyCode), this.UserId, startDate, endDate);
      return await SaveReport(query, cancellationToken);
   }

   [HttpPost("save/xml")]
   public async Task<IActionResult> SaveReportAsXml(
      string currencyCode,
      DateOnly startDate,
      DateOnly endDate,
      CancellationToken cancellationToken)
   {
      var query = new SaveReportXmlQuery(Currency.FromCode(currencyCode), this.UserId, startDate, endDate);
      return await SaveReport(query, cancellationToken);
   }

   private async Task<IActionResult> SaveReport<TQuery>(
      TQuery query,
      CancellationToken cancellationToken)
      where TQuery : IQuery<ReportFile>
   {
      var result = await this.Mediator.Send(query, cancellationToken);

      if (result.IsSuccess && result.Value != null)
      {
         var reportFile = result.Value;
         return File(reportFile.Bytes, reportFile.ContentType, reportFile.FileName);
      }
      else
      {
         return HandleResult(result);
      }
   }
   ```

#### Description of Refactoring:
1. **Extracted Method**:
   - The original code includes two action methods ([`SaveReportAsJson`](https://github.com/Sanchiz1/Financer/blob/main/src/Web/Controllers/ReportsController.cs#L23) and [`SaveReportAsXml`](https://github.com/Sanchiz1/Financer/blob/main/src/Web/Controllers/ReportsController.cs#L34)) responsible for saving reports in JSON and XML formats, respectively.
   - Both action methods perform similar tasks, differing only in the type of query they create (`SaveReportJsonQuery` and `SaveReportXmlQuery`).
   - To eliminate code duplication and improve maintainability, a common private method named [`SaveReport`](https://github.com/Sanchiz1/Financer/blob/main/src/Web/Controllers/ReportsController.cs#L44) is extracted to encapsulate the shared functionality.

2. **Parameterized Method**:
   - The extracted `SaveReport` method is parameterized with a generic type `TQuery`, representing the query type used to save the report.
   - This generic approach allows the method to handle various types of save report queries while maintaining a single implementation.

3. **Enhanced Readability and Maintainability**:
   - By consolidating common functionality into a single method, the code becomes more readable and easier to maintain.
   - Developers can now make changes or enhancements to the report saving logic in a centralized location, avoiding redundant modifications across multiple action methods.

4. **Reusability**:
   - The extracted `SaveReport` method promotes code reusability by enabling other parts of the application to leverage the same report-saving functionality without duplication.
   - Any future additions or modifications to the report-saving process can be applied universally by updating the single `SaveReport` method.

#### Benefits of Refactoring:
- **Elimination of Code Duplication**: The "Extract Method" refactoring eliminates redundant code by consolidating shared functionality into a single method.
- **Improved Maintainability**: Centralizing common logic in the `SaveReport` method simplifies maintenance and reduces the risk of inconsistencies.
- **Enhanced Readability**: The refactoring enhances code readability by promoting a clear and concise structure, making it easier for developers to understand and modify the codebase.

This refactoring aligns with best practices in software development, emphasizing code reuse, maintainability, and readability.


### Replace Method with Method Object

The refactoring technique employed in the provided code snippet is Replace Method with Method Object.

   ```csharp
   public sealed class ReportMakerFacade(CurrencyConversionService currencyConversionService, ICreateReportHandler createReportHandler)
   {
      private readonly CurrencyConversionService _currencyConversionService = currencyConversionService;
      private readonly ICreateReportHandler _createReportHandler = createReportHandler;

      public async Task<Report> CreateReport(Currency preferredCurrency, IEnumerable<Transaction> transactions)
      {
         // Usage of Method Object
         var convertedTransactions = await this._currencyConversionService.ConvertTransactionsAsync(transactions, preferredCurrency);

         return this._createReportHandler.CreateReport(convertedTransactions);
      }
   }

   public class CurrencyConversionService(IExchangeRateProvider providerProxy)
   {
      private readonly IExchangeRateProvider _providerProxy = providerProxy;

      public async Task<IEnumerable<Transaction>> ConvertTransactionsAsync(IEnumerable<Transaction> transactions, Currency targetCurrency)
      {
         // Implementation details
      }

      private async Task<Transaction> ConvertTransactionAsync(Transaction transaction, Currency targetCurrency)
      {
         // Implementation details
      }
   }
   ```

#### Description of Refactoring:
1. **Replace Method with Method Object**:
   - This refactoring technique involves encapsulating a complex method into its own class (method object).
   - In this case, the [`CurrencyConversionService`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/CurrencyConversionService.cs) class encapsulates the logic for converting transactions into a target currency.
   - By extracting this logic into a separate class, the `CurrencyConversionService` maintains a clearer and more focused responsibility, promoting better code organization and readability.

2. **Benefits of Refactoring**:
   - **Improved Maintainability**: By isolating the complex logic into a separate class, the `CurrencyConversionService` becomes easier to maintain and extend.
   - **Enhanced Readability**: The main class ([`ReportMakerFacade`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/ReportMakerFacade.cs)) remains focused on its primary responsibility, while the complexity of currency conversion is abstracted into a dedicated class (`CurrencyConversionService`), leading to clearer and more readable code.
   - **Better Testability**: The extracted method object can be tested independently, facilitating more comprehensive unit testing and improving overall code quality.

#### Example Usage:
- The `CurrencyConversionService` class serves as a method object encapsulating the logic for converting transactions to a target currency. It receives the necessary dependencies and performs the conversion asynchronously.
- The `ReportMakerFacade` class utilizes the `CurrencyConversionService` to convert transactions into the preferred currency before passing them to the `ICreateReportHandler` for report generation. This separation of concerns enhances code clarity and maintainability.

### Replace Temp with Query

The refactoring technique applied in the provided code snippet is Replace Temp with Query.

   ```csharp
   public class BaseApiController : ControllerBase
   {
      private IMediator _mediator;

      protected IMediator Mediator => this._mediator ??= this.HttpContext.RequestServices.GetService<IMediator>()!;

      // `Query` property
      protected string UserId => this.User.GetUserId().ToString();

      // Rest of the code ...
   }

   public class TransactionsController : BaseApiController
   {
      [HttpGet("{id}")]
      public async Task<IActionResult> GetTransaction(
         Guid id, 
         CancellationToken cancellationToken)
      {
         //Usage of the property
         var query = new GetTransactionByIdQuery(this.UserId, id);
         return HandleResult(await this.Mediator.Send(query, cancellationToken));
      }

      [HttpGet("range")]
      public async Task<IActionResult> GetTransactions(
         DateOnly startDate,
         DateOnly endDate,
         CancellationToken cancellationToken)
      {
         //Usage of the property
         var query = new GetTransactionsInDateRangeQuery(this.UserId, startDate, endDate);
         return HandleResult(await this.Mediator.Send(query, cancellationToken));
      }

      [HttpPut("{id}")]
      public async Task<IActionResult> EditTransaction(
         Guid id,
         [FromBody] TransactionDto transactionDto,
         CancellationToken cancellationToken)
      {
         transactionDto = transactionDto with { Id = id };
         //Usage of the property
         var command = new EditTransactionCommand(this.UserId, transactionDto);
         return HandleResult(await this.Mediator.Send(command, cancellationToken));
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> DeleteTransaction(
         Guid id, 
         CancellationToken cancellationToken)
      {
         //Usage of the property
         var command = new DeleteTransactionCommand(this.UserId, id);
         return HandleResult(await this.Mediator.Send(command, cancellationToken));
      }

      // Rest of the code ...
   }
   ```

#### Description of Refactoring:
1. **Replace Temp with Query**:
   - This refactoring technique involves replacing temporary variables with method calls or properties to enhance code readability and maintainability.
   - In this case, the [`TransactionsController`](https://github.com/Sanchiz1/Financer/blob/main/src/Web/Controllers/TransactionsController.cs) class relies on the [`UserId`](https://github.com/Sanchiz1/Financer/blob/main/src/Web/Controllers/BaseApiController.cs#L15) property from its base class [`BaseApiController`](https://github.com/Sanchiz1/Financer/blob/main/src/Web/Controllers/BaseApiController.cs).
   - Instead of storing the `UserId` value in a temporary variable within each method, the `TransactionsController` directly accesses the `UserId` property from its base class.

2. **Benefits of Refactoring**:
   - **Improved Readability**: By directly accessing the `UserId` property from the base class, the code becomes more concise and easier to understand.
   - **Reduced Redundancy**: Eliminates the need to store the `UserId` value in temporary variables within each method, reducing redundancy and potential for errors.
   - **Consistent Behavior**: Ensures consistent behavior across methods by retrieving the `UserId` value from a centralized source, promoting code consistency and maintainability.

#### Example Usage:
- The `TransactionsController` class inherits from the `BaseApiController` class, which contains the `UserId` property.
- Instead of storing the `UserId` value in temporary variables within each method, the `TransactionsController` directly accesses the `UserId` property when needed.
- This approach simplifies the code and improves readability by removing unnecessary temporary variables and promoting consistent access to the `UserId` property.
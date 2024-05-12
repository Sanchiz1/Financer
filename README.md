# Oleksandr Zaitsev

## Patterns

### 1. Builder

### 2. Chain Of Responsibility

### 3. Adapter


## Principles


## Refactoring Techniques



# Illia Kotvitskyi

## Patterns

### 1. Strategy
Used to save reports to a file in JSON or XML formats.

### Implementation

#### Interface and Concrete Implementations
- [**IReportFileSaver**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/SaveReportStrategy/IReportFileSaver.cs): Defines an interface for the report file saving strategy. It declares a method `SaveReport` to save a report.
- [**JsonReportFileSaver**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/SaveReportStrategy/JsonReportFileSaver.cs): Implements `IReportFileSaver` for saving reports in JSON format.
- [**XmlReportFileSaver**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/SaveReportStrategy/XmlReportFileSaver.cs): Implements `IReportFileSaver` for saving reports in XML format.

#### Service Registration
The `IReportFileSaver` implementations are registered in the DI container with unique keys (`"save-report-json"` and `"save-report-xml"`).

#### Controller Methods
- **SaveReportAsJson**: Controller method for saving reports as JSON. It uses a corresponding query handler to handle the saving logic.
- **SaveReportAsXml**: Controller method for saving reports as XML. Similar to the JSON method, it uses a query handler for XML saving.

#### Query Handlers
- **SaveReportJsonQueryHandler**: Handles the saving of reports as JSON. It uses the `JsonReportFileSaver` implementation to save the report.
- **SaveReportXmlQueryHandler**: Handles the saving of reports as XML. It utilizes the `XmlReportFileSaver` implementation for XML saving.

This approach allows for easy extension and modification of report storage formats in the future, adhering to the **open/closed principle** of software design.

### 2. Proxy

The Proxy pattern is utilized in our project to control access to the Yahoo currency API for fetching exchange rates. It allows for additional functionalities, such as caching the retrieved exchange rates to improve performance and reduce unnecessary API calls.

### Implementation

#### Proxy Class
- [**YahooExchangeRateAdapter**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/YahooExchangeRateAdapter.cs): Acts as a proxy (and adapter) for the Yahoo currency API. Implements the `IExchangeRateProvider` interface and internally manages the interaction with the actual Yahoo currency API (`IYahooCurrencyAPI`).

The [`CurrencyConversionService`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/CurrencyConversionService.cs) utilizes the [`IExchangeRateProvider`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/IExchangeRateProvider.cs) interface, which can be a direct implementation or a proxy to fetch exchange rates from the Yahoo currency API. The proxy (`YahooExchangeRateAdapter`) ensures efficient retrieval of exchange rates by caching them and minimizing unnecessary API calls.

This approach enhances performance and reliability by abstracting the complexity of exchange rate retrieval and caching, providing a seamless experience for currency conversion operations.

### 3. Facade
### Purpose
The Facade pattern simplifies the interface to a complex system or set of subsystems by providing a unified interface. In our project, the Facade is utilized to streamline the process of report generation, which involves currency conversion and report creation from a list of transactions.

### Implementation

#### Facade Class
- [**ReportMakerFacade**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/ReportMakerFacade.cs): Acts as a facade to simplify the process of report generation. It abstracts away the complexities of currency conversion and report creation by delegating these tasks to the [`CurrencyConversionService`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/CurrencyConversionService.cs) and [`ICreateReportHandler`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CreateReportHandlers/ICreateReportHandler.cs) implementation, respectively.

## Principles


## Refactoring Techniques
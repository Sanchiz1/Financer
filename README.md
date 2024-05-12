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
- [**YahooCurrencyProxy**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/Yahoo/YahooCurrencyProxy.cs): Acts as a proxy for the Yahoo currency API. Implements the `IYahooCurrencyAPI` interface and internally manages the interaction with the actual Yahoo currency API [`IYahooCurrencyAPI`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/Yahoo/IYahooCurrencyAPI.cs).

The [`CurrencyConversionService`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/CurrencyConversionService.cs) utilizes the [`IExchangeRateProvider`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/IExchangeRateProvider.cs) interface, which can be a direct implementation or a proxy to fetch exchange rates from the Yahoo currency API. The proxy ensures efficient retrieval of exchange rates by caching them and minimizing unnecessary API calls.

This approach enhances performance and reliability by abstracting the complexity of exchange rate retrieval and caching, providing a seamless experience for currency conversion operations.

### 3. Facade
### Purpose
The Facade pattern simplifies the interface to a complex system or set of subsystems by providing a unified interface. In our project, the Facade is utilized to streamline the process of report generation, which involves currency conversion and report creation from a list of transactions.

### Implementation

#### Facade Class
- [**ReportMakerFacade**](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/ReportMakerFacade.cs): Acts as a facade to simplify the process of report generation. It abstracts away the complexities of currency conversion and report creation by delegating these tasks to the [`CurrencyConversionService`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CurrencyConversion/CurrencyConversionService.cs) and [`ICreateReportHandler`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/AggregatesModel/ReportAggregate/CreateReportHandlers/ICreateReportHandler.cs) implementation, respectively.

## Principles

### Fail Fast Principle

The Fail Fast principle suggests that a system should immediately halt execution upon encountering an error or inconsistency, rather than attempting to continue with potentially corrupted data. In the provided code for the [`Money`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/ValueObjects/Money.cs) class, this principle is implemented through the following mechanisms:

1. **Currency Equality Check**: 
   - Before performing arithmetic operations like addition, subtraction, multiplication, and division, the `Money` class checks if the currencies of the operands are equal.
   - If the currencies are not equal, it throws an `InvalidOperationException` with an appropriate error message, halting further execution.

2. **Comparison Operations**:
   - Similar to arithmetic operations, comparison operators such as greater than, less than, greater than or equal to, and less than or equal to are overridden.
   - Before comparing two `Money` objects, the class verifies if their currencies are equal.
   - If the currencies are not equal, it raises an `ArgumentException` with a descriptive error message, adhering to the Fail Fast principle by stopping further processing.

3. **Implicit Conversion**:
   - The class provides an implicit conversion operator to convert `Money` objects to `decimal` values.

By incorporating these checks and validations, the `Money` class ensures that any potential errors related to currency discrepancies are detected and addressed immediately, adhering to the Fail Fast principle.

### KISS Principle

The KISS (Keep It Simple, Stupid) principle advocates for simplicity in design and implementation. The [`Currency`](https://github.com/Sanchiz1/Financer/blob/main/src/Domain/ValueObjects/Currency.cs) class exemplifies this principle through its straightforward structure and functionality:

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


### Dependency Inversion Principle

The Dependency Inversion Principle states that high-level modules should not depend on low-level modules, but both should depend on abstractions. In the `SaveReportXmlQueryHandler` class, this principle is adhered to through the use of interfaces for dependencies.

1. **Interface for `ITransactionRepository`**: The class accepts `ITransactionRepository` as a parameter in its constructor, allowing it to use any implementation of this interface, providing flexibility and the ability to replace dependencies.

2. **Interface for `IReportFileSaver`**: Dependency on `IReportFileSaver` also indicates compliance with this principle. The class depends on an abstraction rather than a specific implementation.

3. **Interface for `ReportMakerFacade`**: Similarly, dependency on `ReportMakerFacade` is through its abstraction, adhering to the Dependency Inversion Principle.

This approach makes it easy to change the implementations of these dependencies without modifying the [`SaveReportXmlQueryHandler`](https://github.com/Sanchiz1/Financer/blob/main/src/Application/UseCases/Reports/SaveReportXml.cs) class, supporting flexibility and ease of making changes.


## Refactoring Techniques

### Extract Method

This technique involves isolating a segment of code into a separate method to improve readability, maintainability, and reusability.

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

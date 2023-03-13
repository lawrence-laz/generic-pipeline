# Exception handling examples
This repository contains examples to handle exceptions in a centralized way, creating a single exception handling behavior that applies to all requests going through the pipeline.

## Usage
To use the exception handling behavior, simply prepend it as the first behavior in the pipeline. This ensures that the behavior has the opportunity to catch all exceptions from the following behaviors and handlers.
```csharp
pipeline.PrependBehavior(new ExceptionHandlingBehavior());
```

To see a complete example, refer to the `ExceptionHandlingExample.cs` file. You can execute this example by running the `dotnet test` command.

## License
This repository is licensed under the MIT License.


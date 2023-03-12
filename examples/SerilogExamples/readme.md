# Serilog Examples
This repository contains examples demonstrating how to implement a single, orthogonal logging behavior that can be reused to log any incoming request and response.

## Usage
There are two ways to use Serilog with the logging behaviors provided in this repository:

1. Serilog logger interfaces: Use the SerilogLoggingBehavior class.
2. Microsoft's ILogger interface: Use the MicrosoftLoggingBehavior class.
To see a complete example, refer to the `LogToFileExamples.cs` file. You can execute this example by running the `dotnet test` command.

## License
This repository is licensed under the MIT License.


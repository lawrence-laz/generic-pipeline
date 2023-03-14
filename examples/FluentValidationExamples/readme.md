# Fluent Validation Examples

This repository provides examples on how to use validators created with the [FluentValidation](https://github.com/FluentValidation/FluentValidation) library as behaviors in the GenericPipeline.

## Usage

To use FluentValidation in your pipeline, you can append the validators to the pipeline as behaviors. Here's an example:
```csharp
using FluentValidation;
using GenericPipeline;

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(customer => customer.FirstName).NotEmpty();
        RuleFor(customer => customer.LastName).NotEmpty();
    }
}

var handler = new RegistrationHandler();
var pipeline = new Pipeline()
    .AppendFluentValidators(new RegisterUserValidator())
    .AppendHandler(handler);

pipeline.Send<RegisterUser, Unit>(new(
    Id: 123,
    FirstName: "", // <-------- Oops, missing first name
    LastName: "Doe",
    Address: "1239 Ward Road, New York, USA"
));
```

For a complete example, refer to the `FluentValidationExample.cs` file. You can execute this example by running the `dotnet test` command.

## License
This repository is licensed under the MIT License.


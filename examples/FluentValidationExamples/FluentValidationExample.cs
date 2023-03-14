using System;
using System.Collections.Generic;
using AutoFixture.Xunit2;
using FluentAssertions;
using FluentValidation;
using GenericPipeline;
using Xunit;

namespace FluentValidationExamples;

public record struct RegisterUser(
    int Id,
    string FirstName,
    string LastName,
    string Address) : IRequest;

public class RegisterUserValidator : AbstractValidator<RegisterUser>
{
    public RegisterUserValidator()
    {
        RuleFor(customer => customer.FirstName).NotEmpty();
        RuleFor(customer => customer.LastName).NotEmpty();
    }
}

public class RegistrationHandler : IRequestHandler<RegisterUser>
{
    public HashSet<int> RegisteredUserIds { get; } = new();

    public Unit Handle(RegisterUser request)
    {
        RegisteredUserIds.Add(request.Id);
        return Unit.Value;
    }
}

public class FluentValidationExample
{
    [Fact]
    public void Sending_a_valid_request_goes_to_handler()
    {
        var handler = new RegistrationHandler();
        var pipeline = new Pipeline()
            .AppendFluentValidators(new RegisterUserValidator())
            .AppendHandler(handler);

        pipeline.Send<RegisterUser, Unit>(new(
            Id: 123,
            FirstName: "John",
            LastName: "Doe",
            Address: "1239 Ward Road, New York, USA"
        ));

        handler.RegisteredUserIds.Should().Contain(123);
    }

    [Fact]
    public void Sending_an_invalid_request_throws()
    {
        var handler = new RegistrationHandler();
        var pipeline = new Pipeline()
            .AppendFluentValidators(new RegisterUserValidator())
            .AppendHandler(handler);

        var act = () =>
            pipeline.Send<RegisterUser, Unit>(new(
                Id: 123,
                FirstName: "", // <-------- Oops, missing first name
                LastName: "Doe",
                Address: "1239 Ward Road, New York, USA"
            ));

        act.Should().Throw<Exception>();

        handler.RegisteredUserIds.Should().NotContain(123);
    }
}


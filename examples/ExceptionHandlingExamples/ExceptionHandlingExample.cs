using System;
using FluentAssertions;
using GenericPipeline;
using Xunit;
using Xunit.Abstractions;

namespace ExceptionHandlingExamples;

public record struct ThrowExceptionRequest() : IRequest;

public class ThrowExceptionRequestHandler : IRequestHandler<ThrowExceptionRequest>
{
    public Unit Handle(ThrowExceptionRequest request)
    {
        throw new Exception("This handler always fails!");
    }
}

public class ExceptionHandlingExample
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<ThrowExceptionRequestHandler>();

        pipeline.PrependBehavior(new ExceptionHandlingBehavior());

        // Act
        var act = () => pipeline.Send<ThrowExceptionRequest, Unit>(new());

        // Assert
        act.Should().NotThrow("because ExceptionHandlingBehavior catches the excpetion");
    }
}

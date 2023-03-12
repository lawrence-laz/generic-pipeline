namespace GenericPipeline.Tests.Sync;

public record struct FirstRequest() : IRequest<string>;

public class FirstRequestHandler : IRequestHandler<FirstRequest, string>
{
    public string Handle(FirstRequest request) => nameof(FirstRequestHandler);
}

public record struct SecondRequest() : IRequest<string>;

public class SecondRequestHandler : IRequestHandler<SecondRequest, string>
{
    public string Handle(SecondRequest request) => nameof(SecondRequestHandler);
}



public class MultipleHandlersTests
{
    [Fact]
    public void Sending_separate_requests_to_separate_handlers_in_a_single_pipeline()
    {
        // Arrange
        var sut = new Pipeline()
            .AppendHandler<FirstRequestHandler>()
            .AppendHandler<SecondRequestHandler>();

        // Act
        var actualFirst = sut.Send<FirstRequest, string>(new());
        var actualSecond = sut.Send<SecondRequest, string>(new());

        // Assert
        actualFirst.Should().Be(nameof(FirstRequestHandler));
        actualSecond.Should().Be(nameof(SecondRequestHandler));
    }
}

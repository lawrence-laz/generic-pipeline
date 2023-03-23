namespace GenericPipeline.Tests.PipelineTests;

using System.Text;

public record struct OrderTestRequest(string Text) : IRequest<string>;

public class OrderTestHandler : IRequestHandler<OrderTestRequest, string>
{
    public string Handle(OrderTestRequest request) => request.Text;
}

public class ABehavior : PipelineBehavior
{
    private readonly StringBuilder _stringBuilder;

    public ABehavior(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        _stringBuilder.Append("A");
        return HandleNext<TRequest, TResponse>(request);
    }
}
public class BBehavior : PipelineBehavior
{
    private readonly StringBuilder _stringBuilder;

    public BBehavior(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        _stringBuilder.Append("B");
        return HandleNext<TRequest, TResponse>(request);
    }
}

public class CBehavior : PipelineBehavior
{
    private readonly StringBuilder _stringBuilder;

    public CBehavior(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        _stringBuilder.Append("C");
        return HandleNext<TRequest, TResponse>(request);
    }
}

public class BehaviorOrderTests
{
    [Theory, AutoData]
    public void AppendBehavior_appends_behavior_to_the_end_of_the_pipeline(
        string expectedHanlderResult)
    {
        // Arrange
        var expectedInvocationOrder = "ABC";
        var actualInvocationOrder = new StringBuilder();
        var sut = new Pipeline()
            .AppendBehavior(new ABehavior(actualInvocationOrder))
            .AppendBehavior(new BBehavior(actualInvocationOrder))
            .AppendBehavior(new CBehavior(actualInvocationOrder))
            .AppendHandler<OrderTestHandler>();

        // Act
        var actualHandlerResult = sut.Send<OrderTestRequest, string>(new(expectedHanlderResult));

        // Assert
        actualHandlerResult.Should().Be(expectedHanlderResult);
        actualInvocationOrder.ToString().Should().Be(expectedInvocationOrder);
    }


    [Theory, AutoData]
    public void PrependBehavior_prepends_behavior_to_the_start_of_the_pipeline(
        string expectedHanlderResult)
    {
        // Arrange
        var expectedInvocationOrder = "CBA";
        var actualInvocationOrder = new StringBuilder();
        var sut = new Pipeline()
            .PrependBehavior(new ABehavior(actualInvocationOrder))
            .PrependBehavior(new BBehavior(actualInvocationOrder))
            .PrependBehavior(new CBehavior(actualInvocationOrder))
            .AppendHandler<OrderTestHandler>();

        // Act
        var actualHandlerResult = sut.Send<OrderTestRequest, string>(new(expectedHanlderResult));

        // Assert
        actualHandlerResult.Should().Be(expectedHanlderResult);
        actualInvocationOrder.ToString().Should().Be(expectedInvocationOrder);
    }
}


namespace GenericPipeline.Tests.PipelineAsyncTests;

using System.Text;

public record struct OrderTestRequest(string Text) : IRequest<string>;

public class OrderTestHandler : IRequestHandlerAsync<OrderTestRequest, string>
{
    public Task<string> Handle(OrderTestRequest request) => Task.FromResult(request.Text);
}

public class ABehavior : PipelineBehaviorAsync
{
    private readonly StringBuilder _stringBuilder;

    public ABehavior(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        _stringBuilder.Append("A");
        return await HandleNext<TRequest, TResponse>(request);
    }
}
public class BBehavior : PipelineBehaviorAsync
{
    private readonly StringBuilder _stringBuilder;

    public BBehavior(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        _stringBuilder.Append("B");
        return await HandleNext<TRequest, TResponse>(request);
    }
}

public class CBehavior : PipelineBehaviorAsync
{
    private readonly StringBuilder _stringBuilder;

    public CBehavior(StringBuilder stringBuilder)
    {
        _stringBuilder = stringBuilder;
    }

    public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request)
    {
        _stringBuilder.Append("C");
        return await HandleNext<TRequest, TResponse>(request);
    }
}

public class BehaviorOrderTests
{
    [Theory, AutoData]
    public async Task AppendBehavior_appends_behavior_to_the_end_of_the_pipeline(
        string expectedHanlderResult)
    {
        // Arrange
        var expectedInvocationOrder = "ABC";
        var actualInvocationOrder = new StringBuilder();
        var sut = new PipelineAsync()
            .AppendBehavior(new ABehavior(actualInvocationOrder))
            .AppendBehavior(new BBehavior(actualInvocationOrder))
            .AppendBehavior(new CBehavior(actualInvocationOrder))
            .AppendHandler<OrderTestHandler>();

        // Act
        var actualHandlerResult = await sut.SendAsync<OrderTestRequest, string>(new(expectedHanlderResult));

        // Assert
        actualHandlerResult.Should().Be(expectedHanlderResult);
        actualInvocationOrder.ToString().Should().Be(expectedInvocationOrder);
    }


    [Theory, AutoData]
    public async Task PrependBehavior_prepends_behavior_to_the_start_of_the_pipeline(
        string expectedHanlderResult)
    {
        // Arrange
        var expectedInvocationOrder = "CBA";
        var actualInvocationOrder = new StringBuilder();
        var sut = new PipelineAsync()
            .PrependBehavior(new ABehavior(actualInvocationOrder))
            .PrependBehavior(new BBehavior(actualInvocationOrder))
            .PrependBehavior(new CBehavior(actualInvocationOrder))
            .AppendHandler<OrderTestHandler>();

        // Act
        var actualHandlerResult = await sut.SendAsync<OrderTestRequest, string>(new(expectedHanlderResult));

        // Assert
        actualHandlerResult.Should().Be(expectedHanlderResult);
        actualInvocationOrder.ToString().Should().Be(expectedInvocationOrder);
    }
}


namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorAsyncTests;

public class MultipleHandlersTests
{
    public record struct RequestA() : IRequest;
    public record struct RequestB() : IRequest<string>;
    public record struct RequestC() : IRequest<int>;

    public class RequestHandlerA : IRequestHandler<RequestA>
    {
        public int HandleCount;
        public void Handle(RequestA request)
        {
            ++HandleCount;
        }
    }

    public class RequestHandlerB : IRequestHandlerAsync<RequestB, string>
    {
        public int HandleCount;
        public async Task<string> Handle(RequestB request, CancellationToken cancellationToken)
        {
            ++HandleCount;
            await Task.Yield();
            return "Hello";
        }
    }

    public class RequestHandlerC : IRequestHandler<RequestC, int>
    {
        public int HandleCount;
        public int Handle(RequestC request)
        {
            ++HandleCount;
            return 1;
        }
    }

    private readonly RequestHandlerA _handlerA;
    private readonly RequestHandlerB _handlerB;
    private readonly RequestHandlerC _handlerC;
    private readonly PipelineAsync _pipeline;

    public MultipleHandlersTests()
    {
        // Arrange
        _handlerA = new RequestHandlerA();
        _handlerB = new RequestHandlerB();
        _handlerC = new RequestHandlerC();
        _pipeline = new PipelineAsync()
            .AppendBehavior(new MediatorBehaviorAsync()
                .AddHandler(_handlerA)
                .AddHandler(_handlerB)
                .AddHandler(_handlerC));
    }

    [Fact]
    public async Task Send_RequestA_calls_RequestHandlerA_only()
    {
        // Act
        await _pipeline.SendAsync<RequestA>(new());

        // Assert
        _handlerA.HandleCount.Should().Be(1);
        _handlerB.HandleCount.Should().Be(0);
        _handlerC.HandleCount.Should().Be(0);
    }

    [Fact]
    public async Task Send_RequestB_calls_RequestHandlerB_only()
    {
        // Act
        var actual = await _pipeline.SendAsync<RequestB, string>(new());

        // Assert
        actual.Should().Be("Hello");
        _handlerA.HandleCount.Should().Be(0);
        _handlerB.HandleCount.Should().Be(1);
        _handlerC.HandleCount.Should().Be(0);
    }

    [Fact]
    public async Task Send_RequestC_calls_RequestHandlerC_only()
    {
        // Act
        var actual = await _pipeline.SendAsync<RequestC, int>(new());

        // Assert
        actual.Should().Be(1);
        _handlerA.HandleCount.Should().Be(0);
        _handlerB.HandleCount.Should().Be(0);
        _handlerC.HandleCount.Should().Be(1);
    }
}


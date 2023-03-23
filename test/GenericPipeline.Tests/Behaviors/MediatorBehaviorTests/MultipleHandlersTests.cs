namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorTests;

public class MultipleHandlersTests
{
    public record struct RequestA() : IRequest;
    public record struct RequestB() : IRequest<string>;
    public record struct RequestC() : IRequest<int>;

    public class RequestHandlerA : IRequestHandler<RequestA>
    {
        public int HandleCount;
        public Unit Handle(RequestA request)
        {
            ++HandleCount;
            return Unit.Value;
        }
    }

    public class RequestHandlerB : IRequestHandler<RequestB, string>
    {
        public int HandleCount;
        public string Handle(RequestB request)
        {
            ++HandleCount;
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
    private readonly Pipeline _pipeline;

    public MultipleHandlersTests()
    {
        // Arrange
        _handlerA = new RequestHandlerA();
        _handlerB = new RequestHandlerB();
        _handlerC = new RequestHandlerC();
        _pipeline = new Pipeline()
            .AppendBehavior(new MediatorBehavior()
                .AddHandler(_handlerA)
                .AddHandler(_handlerB)
                .AddHandler(_handlerC));
    }

    [Fact]
    public void Send_RequestA_calls_RequestHandlerA_only()
    {
        // Act
        var actual = _pipeline.Send<RequestA>(new());

        // Assert
        actual.Should().Be(Unit.Value);
        _handlerA.HandleCount.Should().Be(1);
        _handlerB.HandleCount.Should().Be(0);
        _handlerC.HandleCount.Should().Be(0);
    }

    [Fact]
    public void Send_RequestB_calls_RequestHandlerB_only()
    {
        // Act
        var actual = _pipeline.Send<RequestB, string>(new());

        // Assert
        actual.Should().Be("Hello");
        _handlerA.HandleCount.Should().Be(0);
        _handlerB.HandleCount.Should().Be(1);
        _handlerC.HandleCount.Should().Be(0);
    }

    [Fact]
    public void Send_RequestC_calls_RequestHandlerC_only()
    {
        // Act
        var actual = _pipeline.Send<RequestC, int>(new());

        // Assert
        actual.Should().Be(1);
        _handlerA.HandleCount.Should().Be(0);
        _handlerB.HandleCount.Should().Be(0);
        _handlerC.HandleCount.Should().Be(1);
    }
}


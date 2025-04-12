using System.Linq;

namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorAsyncTests;

public class ParallelSendingTests
{
    public record struct RequestA() : IRequest<string>;
    public record struct RequestB() : IRequest<string>;
    public record struct RequestC() : IRequest<string>;

    public class RequestHandlerA : IRequestHandlerAsync<RequestA, string>
    {
        public int HandleCount;
        public async Task<string> Handle(RequestA request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            ++HandleCount;
            return nameof(RequestHandlerA);
        }
    }

    public class RequestHandlerB : IRequestHandlerAsync<RequestB, string>
    {
        public int HandleCount;
        public async Task<string> Handle(RequestB request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            ++HandleCount;
            return nameof(RequestHandlerB);
        }
    }

    public class RequestHandlerC : IRequestHandlerAsync<RequestC, string>
    {
        public int HandleCount;
        public async Task<string> Handle(RequestC request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            ++HandleCount;
            return nameof(RequestHandlerC);
        }
    }

    private readonly RequestHandlerA _handlerA;
    private readonly RequestHandlerB _handlerB;
    private readonly RequestHandlerC _handlerC;
    private readonly PipelineAsync _pipeline;

    public ParallelSendingTests()
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
    public async Task Sending_parallel_requests_all_are_handled_by_appropriate_handlers()
    {
        // Act
        var tasks = new[] {
            _pipeline.SendAsync<RequestA, string>(new(), CancellationToken.None),
            _pipeline.SendAsync<RequestB, string>(new(), CancellationToken.None),
            _pipeline.SendAsync<RequestC, string>(new(), CancellationToken.None),
        };
        await Task.WhenAll(tasks);

        // Assert
        _handlerA.HandleCount.Should().Be(1);
        _handlerB.HandleCount.Should().Be(1);
        _handlerC.HandleCount.Should().Be(1);
        tasks.Select(task => task.Result).Should().Equal(new[] {
            nameof(RequestHandlerA),
            nameof(RequestHandlerB),
            nameof(RequestHandlerC),
        });
    }
}

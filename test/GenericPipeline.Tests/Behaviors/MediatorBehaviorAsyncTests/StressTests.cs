namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorAsyncTests;

public class StressTests
{
    public interface IRequestWithStringInput : IRequest<string>
    {
        public string Input { get; init; }
    }
    public record Request1(string Input) : IRequestWithStringInput;
    public record Request2(string Input) : IRequestWithStringInput;
    public record Request3(string Input) : IRequestWithStringInput;
    public record Request4(string Input) : IRequestWithStringInput;
    public record Request5(string Input) : IRequestWithStringInput;
    public record Request6(string Input) : IRequestWithStringInput;
    public record Request7(string Input) : IRequestWithStringInput;
    public record Request8(string Input) : IRequestWithStringInput;
    public record Request9(string Input) : IRequestWithStringInput;
    public record Request10(string Input) : IRequestWithStringInput;

    public class RequestHandler1
        : IRequestHandlerAsync<Request1, string>,
          IRequestHandlerAsync<Request2, string>,
          IRequestHandlerAsync<Request3, string>
    {
        public Task<string> Handle(Request1 request, CancellationToken cancellationToken) => Task.FromResult($"{request.Input} - OK");
        public Task<string> Handle(Request2 request, CancellationToken cancellationToken) => Task.FromResult($"{request.Input} - OK");
        public Task<string> Handle(Request3 request, CancellationToken cancellationToken) => Task.FromResult($"{request.Input} - OK");
    }

    public class RequestHandler2
        : IRequestHandler<Request4, string>,
          IRequestHandler<Request5, string>,
          IRequestHandler<Request6, string>
    {
        public string Handle(Request4 request) => $"{request.Input} - OK";
        public string Handle(Request5 request) => $"{request.Input} - OK";
        public string Handle(Request6 request) => $"{request.Input} - OK";
    }

    public class RequestHandler3
        : IRequestHandlerAsync<Request7, string>,
          IRequestHandlerAsync<Request8, string>,
          IRequestHandler<Request9, string>
    {
        public Task<string> Handle(Request7 request, CancellationToken cancellationToken) => Task.FromResult($"{request.Input} - OK");
        public Task<string> Handle(Request8 request, CancellationToken cancellationToken) => Task.FromResult($"{request.Input} - OK");
        public string Handle(Request9 request) => $"{request.Input} - OK";
    }

    public class RequestHandler4 : IRequestHandler<Request10, string>
    {
        public string Handle(Request10 request) => $"{request.Input} - OK";
    }

    public class Behavior1 : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return await HandleNext<TRequest, TResponse>(request, cancellationToken);
        }
    }

    public class Behavior2 : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return await HandleNext<TRequest, TResponse>(request, cancellationToken);
        }
    }

    public class Behavior3 : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return await HandleNext<TRequest, TResponse>(request, cancellationToken);
        }
    }

    [Fact]
    public async Task Calling_mediator_many_times_in_parallel_all_requests_are_handled_without_errors()
    {
        // Arrange
        var pipeline = new PipelineAsync()
            .AppendBehavior<Behavior1>()
            .AppendBehavior<Behavior2>()
            .AppendBehavior<Behavior3>()
            .AppendBehavior(new MediatorBehaviorAsync()
                .AddHandler<RequestHandler1>()
                .AddHandler<RequestHandler2>()
                .AddHandler<RequestHandler3>()
                .AddHandler<RequestHandler4>())
            .ThrowOnUnhandledRequest();
        const int countPerRequest = 1000;
        const int requestTypesCount = 10;
        var fixture = new Fixture();
        var requests1 = fixture.CreateMany<Request1>(countPerRequest);
        var requests2 = fixture.CreateMany<Request2>(countPerRequest);
        var requests3 = fixture.CreateMany<Request3>(countPerRequest);
        var requests4 = fixture.CreateMany<Request4>(countPerRequest);
        var requests5 = fixture.CreateMany<Request5>(countPerRequest);
        var requests6 = fixture.CreateMany<Request6>(countPerRequest);
        var requests7 = fixture.CreateMany<Request7>(countPerRequest);
        var requests8 = fixture.CreateMany<Request8>(countPerRequest);
        var requests9 = fixture.CreateMany<Request9>(countPerRequest);
        var requests10 = fixture.CreateMany<Request10>(countPerRequest);
        var results = new ConcurrentDictionary<IRequestWithStringInput, string>();

        async Task SendRequests<TRequest>(IEnumerable<TRequest> requests)
            where TRequest : IRequestWithStringInput
        {
            foreach (var request in requests)
            {
                var result = await pipeline.SendAsync<TRequest, string>(request, CancellationToken.None);
                results.TryAdd(request, result);
            }
        }

        var tasks = new List<Task>
        {
            SendRequests(requests1),
            SendRequests(requests2),
            SendRequests(requests3),
            SendRequests(requests4),
            SendRequests(requests5),
            SendRequests(requests6),
            SendRequests(requests7),
            SendRequests(requests8),
            SendRequests(requests9),
            SendRequests(requests10),
        };

        // Act
        await Task.WhenAll(tasks);

        // Assert
        results.Count.Should().Be(countPerRequest * requestTypesCount);
        foreach (var result in results)
        {
            result.Value.Should().Be($"{result.Key.Input} - OK");
        }
    }
}

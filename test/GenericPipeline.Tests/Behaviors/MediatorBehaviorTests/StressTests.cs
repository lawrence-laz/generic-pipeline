namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorTests;

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
        : IRequestHandler<Request1, string>,
          IRequestHandler<Request2, string>,
          IRequestHandler<Request3, string>
    {
        public string Handle(Request1 request) => $"{request.Input} - OK";
        public string Handle(Request2 request) => $"{request.Input} - OK";
        public string Handle(Request3 request) => $"{request.Input} - OK";
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
        : IRequestHandler<Request7, string>,
          IRequestHandler<Request8, string>,
          IRequestHandler<Request9, string>
    {
        public string Handle(Request7 request) => $"{request.Input} - OK";
        public string Handle(Request8 request) => $"{request.Input} - OK";
        public string Handle(Request9 request) => $"{request.Input} - OK";
    }

    public class RequestHandler4 : IRequestHandler<Request10, string>
    {
        public string Handle(Request10 request) => $"{request.Input} - OK";
    }

    public class Behavior1 : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    public class Behavior2 : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    public class Behavior3 : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    [Fact]
    public void Calling_mediator_many_times_in_parallel_all_requests_are_handled_without_errors()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendBehavior<Behavior1>()
            .AppendBehavior<Behavior2>()
            .AppendBehavior<Behavior3>()
            .AppendBehavior(new MediatorBehavior()
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

        void SendRequests<TRequest>(IEnumerable<TRequest> requests)
            where TRequest : IRequestWithStringInput
        {
            foreach (var request in requests)
            {
                var result = pipeline.Send<TRequest, string>(request);
                results.TryAdd(request, result);
            }
        }

        var threads = new List<Thread>
        {
            new Thread(() => SendRequests(requests1)),
            new Thread(() => SendRequests(requests2)),
            new Thread(() => SendRequests(requests3)),
            new Thread(() => SendRequests(requests4)),
            new Thread(() => SendRequests(requests5)),
            new Thread(() => SendRequests(requests6)),
            new Thread(() => SendRequests(requests7)),
            new Thread(() => SendRequests(requests8)),
            new Thread(() => SendRequests(requests9)),
            new Thread(() => SendRequests(requests10)),
        };

        // Act
        Parallel.ForEach(threads, thread => thread.Start());
        Parallel.ForEach(threads, thread => thread.Join());

        // Assert
        results.Count.Should().Be(countPerRequest * requestTypesCount);
        foreach (var result in results)
        {
            result.Value.Should().Be($"{result.Key.Input} - OK");
        }
    }
}

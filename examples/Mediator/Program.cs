Console.WriteLine("This is an example of a mediator implementation using the GenericPipeline.");

// We start of by creating the mediator behavior and a couple of handlers.
var mediatorBehavior = new MediatorBehavior()
    .AddHandler(new HelloHandler())
    .AddHandler(new GoodbyeHandler());

// We add the mediator behavior to the pipeline.
// GenericPipeline can take many forms, in this case a single mediator behavior
// makes the pipeline behave as a mediator between the sender and the reciever.
var mediator = new Pipeline().AppendBehavior(mediatorBehavior);

// We can now invoke the mediator with requests.
Console.WriteLine(mediator.Send<HelloRequest, string>(new("John Doe")));
Console.WriteLine(mediator.Send<GoodByeRequest, string>(new("John Doe")));

// This particular implementation allows to modify handlers during runtime.
mediatorBehavior.ReplaceHandler(new FormalGreetingHandler());

// Invoking the same request now provides a different result.
Console.WriteLine(mediator.Send<HelloRequest, string>(new("John Doe")));

// These are the requests used in this example.
public record struct HelloRequest(string Name) : IRequest<string>;
public record struct GoodByeRequest(string Name) : IRequest<string>;

// These are the handlers used in this example.
public class HelloHandler : IRequestHandler<HelloRequest, string>
{
    public string Handle(HelloRequest request)
    {
        return $"Hello, {request.Name}!";
    }
}

public class GoodbyeHandler : IRequestHandler<GoodByeRequest, string>
{
    public string Handle(GoodByeRequest request)
    {
        return $"Goodbye, {request.Name}!";
    }
}

public class FormalGreetingHandler : IRequestHandler<HelloRequest, string>
{
    public string Handle(HelloRequest request)
    {
        return $"Good afternoon, {request.Name}.";
    }
}

// This is the mediator implementation used for this example.
public class MediatorBehavior : PipelineBehavior
{
    private readonly HashSet<object> _requestHandlers = new();

    public MediatorBehavior AddHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        _requestHandlers.Add(handler);
        return this;
    }

    public MediatorBehavior ReplaceHandler<TRequest, TResponse>(IRequestHandler<TRequest, TResponse> handler)
        where TRequest : IRequest<TResponse>
    {
        _requestHandlers.RemoveWhere(handler => handler is IRequestHandler<TRequest, TResponse>);
        AddHandler(handler);
        return this;
    }

    public override TResponse Handle<TRequest, TResponse>(TRequest request)
    {
        var handler = (IRequestHandler<TRequest, TResponse>)_requestHandlers
            .First(handler => handler is IRequestHandler<TRequest, TResponse>);
        return handler.Handle(request);
    }
}


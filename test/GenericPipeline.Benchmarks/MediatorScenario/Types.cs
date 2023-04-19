using Mediator;

namespace GenericPipeline.Benchmarks.MediatorScenario;

public record struct DoWorkRequest() : Mediator.IRequest<Mediator.Unit>;

public sealed class DoWorkHandler : Mediator.IRequestHandler<DoWorkRequest>
{
    public ValueTask<Mediator.Unit> Handle(DoWorkRequest request, CancellationToken cancellationToken)
    {
        Workload.DoWork();
        return ValueTask.FromResult(Mediator.Unit.Value);
    }
}

public class DoWorkBehavior<TMessage, TResponse> : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
    {
        Workload.DoWork();
        return await next(message, cancellationToken);
    }
}

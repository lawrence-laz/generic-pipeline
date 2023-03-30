namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorAsyncTests;

public class UnhandledTests
{
    public record struct RequestWithoutHandler() : IRequest;

    public class BehaviorAfterMediator : PipelineBehaviorAsync
    {
        public bool WasCalled;

        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
        {
            WasCalled = true;
            return await HandleNext<TRequest, TResponse>(request, cancellationToken);
        }
    }

    [Fact]
    public async Task I_can_send_unhandled_requests_without_throwing_an_excpetion()
    {
        // Arrange
        var behaviorAfterMediator = new BehaviorAfterMediator();
        var pipeline = new PipelineAsync()
            .AppendBehavior(new MediatorBehaviorAsync())
            .AppendBehavior(behaviorAfterMediator);

        // Act
        var act = () => pipeline.SendAsync<RequestWithoutHandler>(new());

        // Assert
        await act.Should().NotThrowAsync("because behaviors do not throw by default when requests are unhandled");
        behaviorAfterMediator.WasCalled.Should().BeTrue("because no exception was thrown");
    }

    [Fact]
    public async Task I_can_send_unhandled_requests_with_throwing_an_exception()
    {
        // Arrange
        var behaviorAfterMediator = new BehaviorAfterMediator();
        var pipeline = new PipelineAsync()
            .AppendBehavior(new MediatorBehaviorAsync()).ThrowOnUnhandledRequest()
            .AppendBehavior(behaviorAfterMediator);

        // Act
        var act = () => pipeline.SendAsync<RequestWithoutHandler>(new());

        // Assert
        await act.Should().ThrowAsync<UnhandledRequestException>(
            $"because the pipeline was build with {nameof(PipelineAsyncExtensions.ThrowOnUnhandledRequest)} behavior");
        behaviorAfterMediator.WasCalled.Should().BeFalse("because an exception stops the execution");
    }
}

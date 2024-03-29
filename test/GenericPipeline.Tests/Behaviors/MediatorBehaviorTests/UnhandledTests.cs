namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorTests;

public class UnhandledTests
{
    public record struct RequestWithoutHandler() : IRequest;

    public class BehaviorAfterMediator : PipelineBehavior
    {
        public bool WasCalled;

        public override TResponse Handle<TRequest, TResponse>(TRequest request)
        {
            WasCalled = true;
            return HandleNext<TRequest, TResponse>(request);
        }
    }

    // [Fact]
    // public void Send_unhandled_requests_when_options_are_set_to_throw()
    // {
    //     // Arrange
    //     var options = new HandlerOptions()
    //     {
    //         ThrowUhandledRequestType = true
    //     };
    //     var behaviorAfterMediator = new BehaviorAfterMediator();
    //     var pipeline = new Pipeline()
    //         .AppendBehavior(new MediatorBehavior(options))
    //         .AppendBehavior(behaviorAfterMediator);
    //
    //     // Act
    //     var act = () => pipeline.Send<RequestWithoutHandler, Unit>(new());
    //
    //     // Assert
    //     act.Should().Throw<UnhandledRequestException>();
    //     behaviorAfterMediator.WasCalled.Should().BeFalse();
    // }
    //
    // [Fact]
    // public void Send_unhandled_requests_when_options_are_set_not_to_throw()
    // {
    //     // Arrange
    //     var options = new HandlerOptions()
    //     {
    //         ThrowUhandledRequestType = false
    //     };
    //     var behaviorAfterMediator = new BehaviorAfterMediator();
    //     var pipeline = new Pipeline()
    //         .AppendBehavior(new MediatorBehavior(options))
    //         .AppendBehavior(behaviorAfterMediator);
    //
    //     // Act
    //     var act = () => pipeline.Send<RequestWithoutHandler, Unit>(new());
    //
    //     // Assert
    //     act.Should().NotThrow();
    //     behaviorAfterMediator.WasCalled.Should().BeTrue();
    // }
}

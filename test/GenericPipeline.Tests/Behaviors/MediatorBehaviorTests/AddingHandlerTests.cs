namespace GenericPipeline.Tests.Behaviors.MediatorBehaviorTests;

public class AddingHandlerTestsc
{
    public record struct TestRequest : IRequest;
    public class NotHandler { }
    public class AsyncHandler : IRequestHandlerAsync<TestRequest>
    {
        public Task<Unit> Handle(TestRequest request) => throw new NotImplementedException();
    }

    [Fact]
    public void Adding_non_handler_objects_as_handler_throws_exceptions()
    {
        // Arrange
        var mediator = new MediatorBehavior();

        // Act
        var act = () => mediator.AddHandler(new NotHandler());

        // Assert
        act.Should().Throw<ArgumentException>("because the provided object is not a handler");
    }

    [Fact]
    public void Adding_async_handler_to_non_async_mediator_throws_exceptions()
    {
        // Arrange
        var mediator = new MediatorBehavior();

        // Act
        var act = () => mediator.AddHandler(new AsyncHandler());

        // Assert
        act.Should().Throw<ArgumentException>("because the provided object is not a handler");
    }
}


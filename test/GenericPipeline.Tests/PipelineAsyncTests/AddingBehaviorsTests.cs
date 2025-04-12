namespace GenericPipeline.Tests.PipelineAsyncTests;

public class AddingBehaviorsTestscclass
{
    public class TestBehavior : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            => await HandleNext<TRequest, TResponse>(request, cancellationToken);
    }

    public record struct RequestA() : IRequest;
    public record struct RequestB() : IRequest;

    public class TestHandler :
        IRequestHandler<RequestA>,
        IRequestHandlerAsync<RequestB>
    {
        public Unit Handle(RequestA request) => Unit.Value;
        public Task<Unit> Handle(RequestB request, CancellationToken cancellationToken) => Unit.ValueTask;
    }

    public class NotHandler
    {
    }

    [Fact]
    public void Appending_null_behavrior_throws()
    {
        // Act
        var act = () => new PipelineAsync().AppendBehavior<TestBehavior>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Prepending_null_behavior_throws()
    {
        // Act
        var act = () => new PipelineAsync().PrependBehavior<TestBehavior>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Appending_handler_by_type()
    {
        // Arrange
        var pipeline = new PipelineAsync();

        // Act
        pipeline.AppendHandler(typeof(TestHandler));

        // Assert
        pipeline.GetHandler<TestHandler>().Should().NotBeNull();
    }

    [Fact]
    public void Appending_handler_by_instance()
    {
        // Arrange
        var expected = new TestHandler();
        var pipeline = new PipelineAsync();

        // Act
        pipeline.AppendHandler(expected);

        // Assert
        pipeline.GetHandler<TestHandler>().Should().Be(expected);
    }

    [Fact]
    public void Appending_null_handler_type_throws()
    {
        // Arrange
        var pipeline = new PipelineAsync();

        // Act
        var act = () => pipeline.AppendHandler(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Appending_non_handler_type_throws()
    {
        // Arrange
        var pipeline = new PipelineAsync();

        // Act
        var act = () => pipeline.AppendHandler(typeof(NotHandler));

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
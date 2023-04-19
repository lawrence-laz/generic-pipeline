namespace GenericPipeline.Tests.PipelineTests;

public class AddingBehaviorsTestscclass
{
    public class TestBehavior : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    public record struct RequestA() : IRequest;
    public record struct RequestB() : IRequest;

    public class TestHandler :
        IRequestHandler<RequestA>,
        IRequestHandler<RequestB>
    {
        public void Handle(RequestA request) { }
        public void Handle(RequestB request) { }
    }

    public class NotHandler
    {
    }

    [Fact]
    public void Appending_null_behavrior_throws()
    {
        // Act
        var act = () => new Pipeline().AppendBehavior<TestBehavior>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Prepending_null_behavior_throws()
    {
        // Act
        var act = () => new Pipeline().PrependBehavior<TestBehavior>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Appending_handler_by_type()
    {
        // Arrange
        var pipeline = new Pipeline();

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
        var pipeline = new Pipeline();

        // Act
        pipeline.AppendHandler(expected);

        // Assert
        pipeline.GetHandler<TestHandler>().Should().Be(expected);
    }

    [Fact]
    public void Appending_null_handler_type_throws()
    {
        // Arrange
        var pipeline = new Pipeline();

        // Act
        var act = () => pipeline.AppendHandler(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Appending_non_handler_type_throws()
    {
        // Arrange
        var pipeline = new Pipeline();

        // Act
        var act = () => pipeline.AppendHandler(typeof(NotHandler));

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}

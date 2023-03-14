namespace GenericPipeline.Tests.PipelineTests;

public class AddingBehaviorsTestscclass
{
    public class TestBehavior : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    [Fact]
    public void Appending_behavrior_throws()
    {
        // Act
        var act = () => new Pipeline().AppendBehavior<TestBehavior>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Prepending_behavior_throws()
    {
        // Act
        var act = () => new Pipeline().PrependBehavior<TestBehavior>(null!);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }
}

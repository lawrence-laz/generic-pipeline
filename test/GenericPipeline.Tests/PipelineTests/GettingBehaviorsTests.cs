namespace GenericPipeline.Tests.PipelineTests;

public class GettingBehaviorsTests
{
    public class BehaviorA : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    public class BehaviorB : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    public class BehaviorC : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    public class BehaviorD : PipelineBehavior
    {
        public override TResponse Handle<TRequest, TResponse>(TRequest request) => HandleNext<TRequest, TResponse>(request);
    }

    [Fact]
    public void Get_behaviors_by_type()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendBehavior<BehaviorA>()
            .AppendBehavior<BehaviorB>()
            .AppendBehavior<BehaviorC>()
            .AppendBehavior<BehaviorD>();

        // Act
        var actualA = pipeline.GetBehavior<BehaviorA>();
        var actualB = pipeline.GetBehavior<BehaviorB>();
        var actualC = pipeline.GetBehavior<BehaviorC>();
        var actualD = pipeline.GetBehavior<BehaviorD>();

        // Assert
        actualA.Should().NotBeNull();
        actualB.Should().NotBeNull();
        actualC.Should().NotBeNull();
        actualD.Should().NotBeNull();
    }

    [Fact]
    public void Get_all_behaviors()
    {
        // Arrange
        var expected = new PipelineBehavior[]
        {
            new BehaviorA(),
            new BehaviorB(),
            new BehaviorC(),
            new BehaviorD(),
        };
        var pipeline = new Pipeline()
            .AppendBehavior(expected[0])
            .AppendBehavior(expected[1])
            .AppendBehavior(expected[2])
            .AppendBehavior(expected[3]);

        // Act
        var actual = pipeline.GetBehaviors();

        // Assert
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public void Get_all_behaviors_when_empty()
    {
        // Arrange
        var pipeline = new Pipeline();

        // Act
        var actual = pipeline.GetBehaviors();

        // Assert
        actual.Should().BeEmpty();
    }
}

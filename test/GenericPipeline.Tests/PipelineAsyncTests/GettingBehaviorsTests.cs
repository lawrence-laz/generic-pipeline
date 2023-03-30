namespace GenericPipeline.Tests.PipelineAsyncTests;

public class GettingBehaviorsTests
{
    public class BehaviorA : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            => await HandleNext<TRequest, TResponse>(request, cancellationToken);
    }

    public class BehaviorB : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            => await HandleNext<TRequest, TResponse>(request, cancellationToken);
    }

    public class BehaviorC : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            => await HandleNext<TRequest, TResponse>(request, cancellationToken);
    }

    public class BehaviorD : PipelineBehaviorAsync
    {
        public override async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            => await HandleNext<TRequest, TResponse>(request, cancellationToken);
    }

    [Fact]
    public void Get_behaviors_by_type()
    {
        // Arrange
        var pipeline = new PipelineAsync()
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
        var expected = new PipelineBehaviorAsync[]
        {
            new BehaviorA(),
            new BehaviorB(),
            new BehaviorC(),
            new BehaviorD(),
        };
        var pipeline = new PipelineAsync()
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
        var pipeline = new PipelineAsync();

        // Act
        var actual = pipeline.GetBehaviors();

        // Assert
        actual.Should().BeEmpty();
    }
}

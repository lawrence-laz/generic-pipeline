namespace GenericPipeline.Tests.PipelineTests;

public class GettingHandlerTests
{
    public record struct RequestA : IRequest;
    public record struct RequestB : IRequest;
    public record struct RequestC : IRequest;
    public class RequestHandlerA : IRequestHandler<RequestA>
    {
        public Unit Handle(RequestA request) => throw new NotImplementedException();
    }
    public class RequestHandlerB : IRequestHandler<RequestB>
    {
        public Unit Handle(RequestB request) => throw new NotImplementedException();
    }
    public class RequestHandlerC : IRequestHandler<RequestC>
    {
        public Unit Handle(RequestC request) => throw new NotImplementedException();
    }

    [Fact]
    public void Getting_existing_handler_from_pipeline()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<RequestHandlerA>()
            .AppendHandler<RequestHandlerB>()
            .AppendHandler<RequestHandlerC>();

        // Act
        var actualA = pipeline.GetHandler<RequestHandlerA>();
        var actualB = pipeline.GetHandler<RequestHandlerB>();
        var actualC = pipeline.GetHandler<RequestHandlerC>();

        // Assert
        actualA.Should().BeOfType<RequestHandlerA>();
        actualB.Should().BeOfType<RequestHandlerB>();
        actualC.Should().BeOfType<RequestHandlerC>();
    }


    [Fact]
    public void Getting_existing_handler_from_mediator()
    {
        // Arrange
        var mediator = new MediatorBehavior()
            .AddHandler<RequestHandlerA>()
            .AddHandler<RequestHandlerB>()
            .AddHandler<RequestHandlerC>();
        var pipeline = new Pipeline()
            .AppendBehavior(mediator);

        // Act
        var actualA = pipeline.GetHandler<RequestHandlerA>();
        var actualB = pipeline.GetHandler<RequestHandlerB>();
        var actualC = pipeline.GetHandler<RequestHandlerC>();

        // Assert
        actualA.Should().BeOfType<RequestHandlerA>();
        actualB.Should().BeOfType<RequestHandlerB>();
        actualC.Should().BeOfType<RequestHandlerC>();
    }

    [Fact]
    public void Getting_non_existing_handler_from_pipeline_throws()
    {
        // Arrange
        var pipeline = new Pipeline()
            .AppendHandler<RequestHandlerA>()
            .AppendHandler<RequestHandlerB>();

        // Act
        var act = () => pipeline.GetHandler<RequestHandlerC>();

        // Assert
        act.Should().ThrowExactly<HandlerNotFoundException>();
    }
}


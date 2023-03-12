using System;
using System.IO;
using AutoFixture.Xunit2;
using FluentAssertions;
using GenericPipeline;
using Microsoft.Extensions.Logging;
using Serilog;
using Xunit;

namespace SerilogExamples;

public record struct MyRequest() : IRequest<string>;
public record struct MyOtherRequest() : IRequest<string>;

public class MyRequestHandler
    : IRequestHandler<MyRequest, string>,
    IRequestHandler<MyOtherRequest, string>
{
    public string Handle(MyRequest request) => $"Hello from {nameof(MyRequestHandler)}!";
    public string Handle(MyOtherRequest request) => $"Hello again from {nameof(MyRequestHandler)}!";
}

public class LogToFileExamples : IDisposable
{
    private string? _filePath;

    public void Dispose()
    {
        if (_filePath is not null)
        {
            File.Delete(_filePath);
        }
    }

    [Theory, AutoData]
    public void Logging_behavior_using_Serilog_interface(string filePath)
    {
        // Arrange
        _filePath = filePath;
        var logger = new LoggerConfiguration()
            .WriteTo.File(filePath, outputTemplate: "{Message}")
            .CreateLogger();
        var pipeline = new Pipeline()
            .AppendBehavior(new SerilogLoggingBehavior(logger))
            .AppendHandler<MyRequestHandler>();

        // Act
        pipeline.Send<MyRequest, string>(new());
        pipeline.Send<MyOtherRequest, string>(new());

        // Assert
        File.ReadAllLines(filePath)
            .Should()
            .BeEquivalentTo(new[]
            {
                "Recieved \"MyRequest { }\" and responded with \"Hello from MyRequestHandler!\"",
                "Recieved \"MyOtherRequest { }\" and responded with \"Hello again from MyRequestHandler!\"",
            });
    }

    [Theory, AutoData]
    public void Logging_behavior_using_Microsoft_interface(string filePath)
    {
        // Arrange
        _filePath = filePath;
        var serilogLogger = new LoggerConfiguration()
            .WriteTo.File(filePath, outputTemplate: "{Message}")
            .CreateLogger();
        var microsoftLogger = new LoggerFactory()
            .AddSerilog(serilogLogger)
            .CreateLogger("MyLogger");
        var pipeline = new Pipeline()
            .AppendBehavior(new MicrosoftLoggingBehavior(microsoftLogger))
            .AppendHandler<MyRequestHandler>();

        // Act

        pipeline.Send<MyRequest, string>(new());
        pipeline.Send<MyOtherRequest, string>(new());

        // Assert
        File.ReadAllLines(filePath)
            .Should()
            .BeEquivalentTo(new[]
            {
                "Recieved \"MyRequest { }\" and responded with \"Hello from MyRequestHandler!\"",
                "Recieved \"MyOtherRequest { }\" and responded with \"Hello again from MyRequestHandler!\"",
            });
    }
}


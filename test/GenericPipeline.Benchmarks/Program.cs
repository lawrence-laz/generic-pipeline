using System.Reflection;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using GenericPipeline;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using GenericPipelineScenario = GenericPipeline.Benchmarks.GenericPipelineScenario;
using MediatrScenario = GenericPipeline.Benchmarks.MediatrScenario;
using MethodCallScenario = GenericPipeline.Benchmarks.MethodCallScenario;

BenchmarkRunner.Run<BenchmarkTest>();

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class BenchmarkTest
{
    [GlobalSetup]
    public void Setup()
    {
        SetupGenericPipeline();
        SetupMediatr();
        SetupMediatrSingleton();
    }

    [Benchmark(Baseline = true)]
    public void MethodCall()
    {
        MethodCallScenario.StaticMethods.DoWorkBehavior();
        MethodCallScenario.StaticMethods.DoWorkRequest();
    }

    Pipeline _pipeline;
    private void SetupGenericPipeline()
    {
        _pipeline = new Pipeline()
            .AppendBehavior<GenericPipelineScenario.DoWorkBehavior>()
            .AppendHandler<GenericPipelineScenario.DoWorkHandler>();
    }

    [Benchmark]
    public void GenericPipeline()
    {
        _pipeline.Send<GenericPipelineScenario.DoWorkRequest, GenericPipeline.Unit>(new());
    }

    [Benchmark]
    public void GenericPipeline_MediatR_Style()
    {
        _pipeline.Send(new GenericPipelineScenario.DoWorkRequest());
    }

    IServiceProvider _services;
    IMediator _mediator;
    private void SetupMediatr()
    {
        _services = new ServiceCollection()
            .AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(Program).Assembly);
                options.AddOpenBehavior(typeof(MediatrScenario.DoWorkBehavior<,>));
            })
            .BuildServiceProvider();
        _mediator = _services.GetRequiredService<IMediator>();
    }

    [Benchmark]
    public void MediatR()
    {
        _mediator.Send(new MediatrScenario.DoWorkRequest());
    }

    IServiceProvider _servicesMediatrSingleton;
    IMediator _mediatorSingleton;
    private void SetupMediatrSingleton()
    {
        _servicesMediatrSingleton = new ServiceCollection()
            .AddMediatR(options =>
            {
                options.Lifetime = ServiceLifetime.Singleton;
                options.RegisterServicesFromAssembly(typeof(Program).Assembly);
                options.AddOpenBehavior(typeof(MediatrScenario.DoWorkBehavior<,>));
            })
            .BuildServiceProvider();
        _mediatorSingleton = _servicesMediatrSingleton.GetRequiredService<IMediator>();
    }

    [Benchmark]
    public void MediatR_Singleton()
    {
        _mediatorSingleton.Send(new MediatrScenario.DoWorkRequest());
    }
}

public static class BaseBehaviorExtensions
{
    // This provides a MediatR style sending, where response is infered.
    // It is slower however and requires reflection.
    public static TResponse Send<TResponse>(this Pipeline pipeline, GenericPipeline.IRequest<TResponse> request)
    {
        var method = pipeline
            .GetType()
            .GetMethod(nameof(Pipeline.Send), BindingFlags.Public | BindingFlags.Instance)
            ?.MakeGenericMethod(request.GetType(), typeof(TResponse))
            ?? throw new NotSupportedException();

        var response = method.Invoke(pipeline, new[] { request });

        return response is null
            ? default
            : (TResponse)response;
    }
}


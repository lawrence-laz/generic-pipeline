using System.Reflection;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

using GenericPipeline;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using PipelineNet.MiddlewareResolver;

using GenericPipelineAsyncScenario = GenericPipeline.Benchmarks.GenericPipelineAsyncScenario;
using GenericPipelineScenario = GenericPipeline.Benchmarks.GenericPipelineScenario;
using MediatrAsyncScenario = GenericPipeline.Benchmarks.MediatrScenarioAsync;
using MediatrScenario = GenericPipeline.Benchmarks.MediatrScenario;
using MethodCallScenario = GenericPipeline.Benchmarks.MethodCallScenario;
using PipelineNetAsyncScenario = GenericPipeline.Benchmarks.PipelineNetAsyncScenario;
using PipelineNetScenario = GenericPipeline.Benchmarks.PipelineNetScenario;

BenchmarkRunner.Run<BenchmarkTest>();
// var test = new BenchmarkTest();
// test.Setup();
// await test.GenericPipelineAsync();

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class BenchmarkTest
{
    [GlobalSetup]
    public void Setup()
    {
        SetupGenericPipeline();
        SetupGenericPipelineAsync();
        SetupMediatr();
        SetupMediatrAsync();
        SetupPipelineNet();
        SetupPipelineNetAsync();
    }

    [Benchmark(Baseline = true)]
    public void MethodCall()
    {
        MethodCallScenario.StaticMethods.DoWorkBehavior();
        MethodCallScenario.StaticMethods.DoWorkRequest();
    }

    [Benchmark()]
    public async Task MethodCallAsync()
    {
        await MethodCallScenario.StaticMethods.DoWorkBehaviorAsync();
        await MethodCallScenario.StaticMethods.DoWorkRequestAsync();
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


    PipelineAsync _pipelineAsync;
    private void SetupGenericPipelineAsync()
    {
        _pipelineAsync = new PipelineAsync()
            .AppendBehavior<GenericPipelineAsyncScenario.DoWorkBehavior>()
            .AppendHandler<GenericPipelineAsyncScenario.DoWorkHandler>();
    }

    [Benchmark]
    public async Task GenericPipelineAsync()
    {
        await _pipelineAsync.Send<GenericPipelineAsyncScenario.DoWorkRequest, GenericPipeline.Unit>(new());
    }

    // [Benchmark]
    // public void GenericPipeline_MediatR_Style()
    // {
    //     _pipeline.Send(new GenericPipelineScenario.DoWorkRequest());
    // }

    IServiceProvider _services;
    IMediator _mediator;
    private void SetupMediatr()
    {
        _services = new ServiceCollection()
            .AddMediatR(options =>
            {
                options.Lifetime = ServiceLifetime.Singleton;
                options.RegisterServicesFromAssembly(typeof(Program).Assembly);
                options.AddOpenBehavior(typeof(MediatrScenario.DoWorkBehavior<,>));
            })
            .BuildServiceProvider();
        _mediator = _services.GetRequiredService<IMediator>();
    }

    [Benchmark]
    public async Task MediatR()
    {
        await _mediator.Send(new MediatrScenario.DoWorkRequest());
    }

    IServiceProvider _servicesMediatrAsync;
    IMediator _mediatorAsync;
    private void SetupMediatrAsync()
    {
        _servicesMediatrAsync = new ServiceCollection()
            .AddMediatR(options =>
            {
                options.Lifetime = ServiceLifetime.Singleton;
                options.RegisterServicesFromAssembly(typeof(Program).Assembly);
                options.AddOpenBehavior(typeof(MediatrAsyncScenario.DoWorkBehavior<,>));
            })
            .BuildServiceProvider();
        _mediatorAsync = _servicesMediatrAsync.GetRequiredService<IMediator>();
    }

    [Benchmark]
    public async Task MediatRAsync()
    {
        await _mediatorAsync.Send(new MediatrAsyncScenario.DoWorkRequest());
    }

    PipelineNet.Pipelines.IPipeline<PipelineNetScenario.DoWorkRequest> _pipelineNet;
    private void SetupPipelineNet()
    {
        _pipelineNet = new PipelineNet.Pipelines.Pipeline<PipelineNetScenario.DoWorkRequest>(new ActivatorMiddlewareResolver())
            .Add<PipelineNetScenario.DoWorkBehavior>()
            .Add<PipelineNetScenario.DoWorkHandler>();
    }

    [Benchmark]
    public void PipelineNet()
    {
        _pipelineNet.Execute(new());
    }


    PipelineNet.Pipelines.IAsyncPipeline<PipelineNetAsyncScenario.DoWorkRequest> _pipelineNetAsync;
    private void SetupPipelineNetAsync()
    {
        _pipelineNetAsync = new PipelineNet.Pipelines.AsyncPipeline<PipelineNetAsyncScenario.DoWorkRequest>(new ActivatorMiddlewareResolver())
            .Add<PipelineNetAsyncScenario.DoWorkBehavior>()
            .Add<PipelineNetAsyncScenario.DoWorkHandler>();
    }

    [Benchmark]
    public async Task PipelineNetAsync()
    {
        await _pipelineNetAsync.Execute(new());
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


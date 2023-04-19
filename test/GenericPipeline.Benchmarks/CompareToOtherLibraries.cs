using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

using GenericPipeline;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using PipelineNet.MiddlewareResolver;

using GenericPipelineAsyncScenario = GenericPipeline.Benchmarks.GenericPipelineAsyncScenario;
using GenericPipelineScenario = GenericPipeline.Benchmarks.GenericPipelineScenario;
using MediatrAsyncScenario = GenericPipeline.Benchmarks.MediatrScenarioAsync;
using MediatrScenario = GenericPipeline.Benchmarks.MediatrScenario;
using MediatorScenario = GenericPipeline.Benchmarks.MediatorScenario;
using MediatorAsyncScenario = GenericPipeline.Benchmarks.MediatorAsyncScenario;
using MethodCallScenario = GenericPipeline.Benchmarks.MethodCallScenario;
using PipelineNetAsyncScenario = GenericPipeline.Benchmarks.PipelineNetAsyncScenario;
using PipelineNetScenario = GenericPipeline.Benchmarks.PipelineNetScenario;

[SimpleJob(RuntimeMoniker.Net60)]
[MemoryDiagnoser]
public class CompareToOtherLibraries
{
    [GlobalSetup]
    public void Setup()
    {
        SetupGenericPipeline();
        // SetupGenericPipelineAsync();
        // SetupMediatr();
        // SetupMediatrAsync();
        SetupMediator();
        // SetupMediatorAsync();
        // SetupPipelineNet();
        // SetupPipelineNetAsync();
    }

    [Benchmark(Baseline = true)]
    public void MethodCall()
    {
        MethodCallScenario.StaticMethods.DoWorkBehavior();
        MethodCallScenario.StaticMethods.DoWorkRequest();
    }

/*     [Benchmark]
    public async Task MethodCallAsync()
    {
        await MethodCallScenario.StaticMethods.DoWorkBehaviorAsync();
        await MethodCallScenario.StaticMethods.DoWorkRequestAsync();
    } */

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

/*     [Benchmark]
    public void GenericPipelineObject()
    {
        object request = new GenericPipelineScenario.DoWorkRequest();
        _pipeline.Send(request);
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
        await _pipelineAsync.SendAsync<GenericPipelineAsyncScenario.DoWorkRequest, GenericPipeline.Unit>(new());
    } */

    // [Benchmark]
    // public void GenericPipeline_MediatR_Style()
    // {
    //     _pipeline.Send(new GenericPipelineScenario.DoWorkRequest());
    // }

/*     IServiceProvider _mediatrServices;
    IMediator _mediatrMediator;
    private void SetupMediatr()
    {
        _mediatrServices = new ServiceCollection()
            .AddMediatR(options =>
            {
                options.Lifetime = ServiceLifetime.Singleton;
                options.RegisterServicesFromAssembly(typeof(Program).Assembly);
                options.AddOpenBehavior(typeof(MediatrScenario.DoWorkBehavior<,>));
            })
            .BuildServiceProvider();
        _mediatrMediator = _mediatrServices.GetRequiredService<IMediator>();
    }

    [Benchmark]
    public async Task MediatR()
    {
        await _mediatrMediator.Send(new MediatrScenario.DoWorkRequest());
    } */

    IServiceProvider _mediatorServices;
    Mediator.Mediator _mediatorMediator;
    private void SetupMediator()
    {
        _mediatorServices = new ServiceCollection()
            .AddMediator(options =>
            {
                options.Namespace = "Mediator";
                options.ServiceLifetime = ServiceLifetime.Singleton;
            })
            .AddSingleton(typeof(Mediator.IPipelineBehavior<,>), typeof(MediatorScenario.DoWorkBehavior<,>))
            .BuildServiceProvider();

        _mediatorMediator = _mediatorServices.GetRequiredService<Mediator.Mediator>();
    }

    [Benchmark]
    public ValueTask<Mediator.Unit> Mediator()
    {
        return _mediatorMediator.Send(new MediatorScenario.DoWorkRequest());
    }

/*     IServiceProvider _mediatorAsyncServices;
    Mediator.Mediator _mediatorAsyncMediator;
    private void SetupMediatorAsync()
    {
        _mediatorAsyncServices = new ServiceCollection()
            .AddMediator(options =>
            {
                options.Namespace = "Mediator";
                options.ServiceLifetime = ServiceLifetime.Singleton;
            })
            .AddSingleton(typeof(Mediator.IPipelineBehavior<,>), typeof(MediatorAsyncScenario.DoWorkBehavior<,>))
            .BuildServiceProvider();

        _mediatorMediator = _mediatorAsyncServices.GetRequiredService<Mediator.Mediator>();
    }

    [Benchmark]
    public async ValueTask<Mediator.Unit> MediatorAsync()
    {
        return await _mediatorAsyncMediator.Send(new MediatorScenario.DoWorkRequest());
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
    } */
}


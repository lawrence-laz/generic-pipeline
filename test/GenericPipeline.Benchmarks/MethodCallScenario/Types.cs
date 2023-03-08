namespace GenericPipeline.Benchmarks.MethodCallScenario;

public static class StaticMethods
{
    public static void DoWorkRequest()
    {
        Workload.DoWork();
    }

    public static void DoWorkBehavior()
    {
        Workload.DoWork();
    }

    public static async Task DoWorkRequestAsync()
    {
        await Workload.DoWorkAsync();
    }

    public static async Task DoWorkBehaviorAsync()
    {
        await Workload.DoWorkAsync();
    }
}


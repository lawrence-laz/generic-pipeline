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
}


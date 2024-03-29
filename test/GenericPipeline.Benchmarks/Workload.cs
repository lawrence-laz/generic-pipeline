namespace GenericPipeline.Benchmarks;

public static class Workload
{
    public static void DoWork()
    {
        // Some arbitrary operations to be invoked in various ways.
        int x = 0;
        for (int i = 0; i < 2; ++i)
            x += i % 3;
    }

    public static async Task DoWorkAsync()
    {
        await Task.Yield();
        // Some arbitrary operations to be invoked in various ways.
        int x = 0;
        for (int i = 0; i < 2; ++i)
            x += i % 3;
    }
}


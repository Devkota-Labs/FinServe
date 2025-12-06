//using System.Diagnostics;

//namespace Shared.Logging;

//public class PerformanceLogger(ILogProvider logger)
//{
//    private readonly ILogProvider _logger = logger;

//    public T Measure<T>(Func<T> func, string name)
//    {
//        var sw = Stopwatch.StartNew();
//        var result = func();
//        sw.Stop();
//        _logger.Information($"Performance: {name} took {sw.ElapsedMilliseconds} ms");
//        return result;
//    }
//}

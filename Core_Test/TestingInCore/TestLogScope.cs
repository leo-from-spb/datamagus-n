using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Core.TestingInCore;

public sealed class TestLogScope : IDisposable
{
    private const string LogLayout =
        "${level:uppercase=true}|${logger}|${message}${onexception: ${exception:format=tostring}}";

    private readonly MemoryTarget          MemoryTarget;

    public TestLogScope()
    {
        MemoryTarget = new MemoryTarget("test-memory")
                       {
                           Layout = LogLayout
                       };

        var config = new LoggingConfiguration();

        config.AddRule(minLevel: LogLevel.Trace,
                       maxLevel: LogLevel.Fatal,
                       target: MemoryTarget);

        LogManager.Configuration = config;
    }

    public IList<string> Lines => MemoryTarget.Logs;

    public bool HasWarningsOrHigher =>
        Lines.Any(line => line.StartsWith("WARN|") ||
                          line.StartsWith("ERROR|") ||
                          line.StartsWith("FATAL|"));

    public void Dispose()
    {
        LogManager.Flush();
    }
}

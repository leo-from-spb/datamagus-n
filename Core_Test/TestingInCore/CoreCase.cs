using NUnit.Framework.Interfaces;

namespace Core.TestingInCore;

public abstract class CoreCase
{
    private TestLogScope logScope = null!;

    [SetUp]
    public void BaseSetUp()
    {
        logScope = new TestLogScope();
    }

    [TearDown]
    public void BaseTearDown()
    {
        try
        {
            bool testFailed =
                TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed;

            bool logContainsWarnings =
                logScope.HasWarningsOrHigher;

            if (logContainsWarnings)
            {
                TestContext.Out.WriteLine("Test produced warning/error/fatal logs.");
            }

            if (testFailed || logContainsWarnings)
            {
                TestContext.Out.WriteLine("===== LOG BEGIN =====");
                foreach (string line in logScope.Lines) TestContext.Out.WriteLine(line);
                TestContext.Out.WriteLine("===== LOG END =====");
            }

            if (logContainsWarnings)
            {
                Assert.Fail("Test produced warning/error/fatal log entries.");
            }
        }
        finally
        {
            logScope.Dispose();
        }
    }
}



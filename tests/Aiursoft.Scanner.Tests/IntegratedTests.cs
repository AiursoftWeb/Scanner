using Microsoft.Extensions.DependencyInjection;


namespace Aiursoft.Scanner.Tests;

[TestClass]
public class IntegratedTests
{
    [TestMethod]
    public void TestScanLibrary()
    {
        var services = new ServiceCollection();
        services.AddLibraryDependencies();
        var sp = services.BuildServiceProvider();
        var mss = sp.GetRequiredService<MySampleService>();
        Assert.IsNotNull(mss);
    }

    [TestMethod]
    public void TestScanEntry()
    {
        var services = new ServiceCollection();
        services.AddScannedDependencies();
        var sp = services.BuildServiceProvider();
        var mss = sp.GetService<MySampleService>();

        // Can't load this. Because entry is UT.
        Assert.IsNull(mss);
    }

    [TestMethod]
    public void TestScanAssemble()
    {
        var services = new ServiceCollection();
        services.AddAssemblyDependencies(new IntegratedTests().GetType().Assembly);
        var sp = services.BuildServiceProvider();
        var mss = sp.GetRequiredService<MySampleService>();

        // Can load this. Because passed the assembly: IntegratedTests.
        Assert.IsNotNull(mss);
    }

    [TestMethod]
    public void TestScanAbstracts()
    {
        var services = new ServiceCollection();
        services.AddLibraryDependencies(typeof(IMySampleInterface));
        var sp = services.BuildServiceProvider();
        var mss = sp.GetRequiredService<IMySampleInterface>();
        Assert.IsNotNull(mss);
        Assert.AreEqual(typeof(MySampleImplementation), mss.GetType());
    }
}

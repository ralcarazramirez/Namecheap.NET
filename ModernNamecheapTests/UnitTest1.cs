using Microsoft.Extensions.DependencyInjection;
using NameCheap;

namespace ModernNamecheapTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddOptions<NamecheapOptions>();
    }

    [Test]
    public void Test1()
    {
        Assert.Pass();
    }
}
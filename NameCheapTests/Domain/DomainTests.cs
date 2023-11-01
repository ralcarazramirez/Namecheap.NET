using System.Linq;

namespace NameCheapTests.Domain;

[TestClass]
public class DomainTests : TestBase
{
    [TestMethod]
    public async Task GetInfo_ReturnsInformationOnExistingDomain()
    {
        var info = await _api.Domains.GetInfo(_domainName);
        Assert.IsTrue(info.ID > 0);
    }

    [TestMethod]
    public async Task GetList_ShouldContainTheTestDomain()
    {
        var result = await _api.Domains.GetList();
        Assert.IsTrue(result.Domains.Length > 0);
        Assert.IsTrue(result.Domains.Any(d => string.Equals(d.Name, _domainName)));
    }

    [TestMethod, Ignore("Needs work - can only renew a domain so many times")]
    public async Task Test_renew()
    {
        var result = await _api.Domains.Renew(_domainName, 1);

        Assert.AreEqual(result.DomainName, _domainName);
        Assert.IsTrue(result.DomainID > 0);
        Assert.AreEqual(result.Renew, true);
        Assert.IsTrue(result.OrderID > 0);
        Assert.IsTrue(result.TransactionID > 0);
        Assert.IsTrue(result.ChargedAmount > 0);
    }

    [TestMethod, Ignore("Needs work - can only reactivate an expired domain")]
    public async Task Test_reactivate()
    {
        var result = await _api.Domains.Reactivate(_domainName);

        Assert.AreEqual(result.DomainName, _domainName);
        Assert.AreEqual(result.IsSuccess, true);
        Assert.IsTrue(result.OrderID > 0);
        Assert.IsTrue(result.TransactionID > 0);
        Assert.IsTrue(result.ChargedAmount > 0);
    }
}
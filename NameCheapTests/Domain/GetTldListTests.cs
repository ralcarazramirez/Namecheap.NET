﻿using System.Linq;

namespace NameCheapTests.Domain;

[TestClass]
public class GetTldListTests : TestBase
{
    [TestMethod]
    public async Task GetTldList_ReturnsMultipleResults()
    {
        var result = await _api.Domains.GetTldList();

        Assert.IsTrue(result.Tlds.Length > 400); // 462 on 2019-04-04
        var canada = result.Tlds.Single(o => o.Name == "ca");
        Assert.AreEqual("Canada Country TLD", canada.LongName, ignoreCase: true);
        Assert.AreEqual("A", canada.Category);
        Assert.IsTrue(canada.IsApiRegisterable);
        Assert.IsTrue(canada.IsApiTransferable);
        Assert.IsTrue(canada.IsEppRequired);
        Assert.IsFalse(canada.IsSupportsIDN);
        Assert.AreEqual(10, canada.MaxRegisterYears);
        Assert.AreEqual(9, canada.MaxRenewYears);
        Assert.AreEqual(1, canada.MaxTransferYears);
        Assert.AreEqual(1, canada.MinRegisterYears);
        Assert.AreEqual(1, canada.MinRenewYears);
        Assert.AreEqual(1, canada.MinTransferYears);
    }
}
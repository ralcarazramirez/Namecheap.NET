﻿using System.Linq;
using NameCheap.Dns;

namespace NameCheapTests.Dns;

[TestClass]
public class HostTests : TestBase
{
    [TestInitialize]
    public async Task BeforeEachTest()
    {
        var (secondLevel, tld) = DomainParts.Value;

        // resets DNS to default
        await _api.Dns.SetDefault(secondLevel, tld);

        // resets all host entries
        await _api.Dns.SetHosts(
            secondLevel,
            tld,
            new HostEntry[0]).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task SetHosts_SetsMultipleEntries()
    {
        var (secondLevel, tld) = DomainParts.Value;
        await _api.Dns.SetHosts(
            secondLevel,
            tld,
            new HostEntry[] {
                new HostEntry {
                    Address = "1.1.1.1",
                    HostName = "@",
                    RecordType =  RecordType.A,
                    Ttl = "2000",
                },
                new HostEntry {
                    Address = _domainName,
                    HostName = "www",
                    RecordType =  RecordType.CNAME
                }
            });

        var hosts = await _api.Dns.GetHosts(secondLevel, tld);

        Assert.AreEqual(2, hosts.HostEntries.Length);

        var entry1 = hosts.HostEntries.FirstOrDefault(h => h.RecordType == RecordType.A);
        Assert.AreEqual("1.1.1.1", entry1.Address);
        Assert.AreEqual("@", entry1.HostName);
        Assert.AreEqual("2000", entry1.Ttl);

        var entry2 = hosts.HostEntries.FirstOrDefault(h => h.RecordType == RecordType.CNAME);
        Assert.AreEqual($"{_domainName}.", entry2.Address, "Because the CNAME gets a . at the end.");
        Assert.AreEqual("www", entry2.HostName);
        Assert.AreEqual("1800", entry2.Ttl);
    }

    [TestMethod]
    public void SetHosts_Errors_WhenCnameIsAnIp()
    {
        Assert.Inconclusive("Tests needs to be written.");
    }
}
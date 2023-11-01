using System.Linq;
using NameCheap.Dns;

namespace NameCheapTests.Dns;

[TestClass]
public class EmailForwardingTests : TestBase
{
    private const string TestMailbox = "namecheapdotnet";


    [TestInitialize]
    public void BeforeEachTest()
    {
        
        // sets all forwards to default value
        _api.Dns.DeleteAllEmailForwarding(_domainName);
    }

    [TestMethod]
    public async Task SetEmailForward_SetsForward()
    {
        var timeNonce = DateTime.Now.Ticks.ToString();
        string nonceMailBox = $"mb{timeNonce}",
            nonceForward = $"fwd{timeNonce}@example.net";
        var forwards = new[] { new EmailForwarding { MailBox = nonceMailBox, ForwardTo = nonceForward }};

        // Act
        await _api.Dns.SetEmailForwarding(_domainName, forwards);

        // Assert
        var allForwards = await _api.Dns.GetEmailForwarding(_domainName);
        var forwardCounts = allForwards.Emails.Count(e => 
            string.Equals(e.MailBox, nonceMailBox, StringComparison.OrdinalIgnoreCase)
            && string.Equals(e.ForwardTo, nonceForward, StringComparison.OrdinalIgnoreCase));
        Assert.AreEqual(1, forwardCounts);
    }

    [TestMethod]
    public async Task SetEmailForward_SetsMultipleRecipients()
    {
        var timeNonce = DateTime.Now.Ticks.ToString();
        string nonceMailBox = $"mb{timeNonce}",
            nonceFwd1 = $"fwd1_{timeNonce}@example.com",
            nonceFwd2 = $"fwd2_{timeNonce}@example.com";

        var forwards = new[] {
            new EmailForwarding { MailBox = nonceMailBox, ForwardTo = nonceFwd1 },
            new EmailForwarding { MailBox = nonceMailBox, ForwardTo = nonceFwd2 },
        };

        // Act
        await _api.Dns.SetEmailForwarding(_domainName, forwards);

        // Assert
        var allForwards = await _api.Dns.GetEmailForwarding(_domainName);
        Assert.AreEqual(2, allForwards.Emails.Count, "Expected two entries (for one mailbox).");
        Assert.IsTrue(
            allForwards.Emails.All(e => string.Equals(e.MailBox, nonceMailBox, StringComparison.OrdinalIgnoreCase)),
            message: "Expected all forwards to have the same mailbox.");
        Assert.AreEqual(
            1,
            allForwards.Emails.Count(e => string.Equals(e.ForwardTo, nonceFwd1, StringComparison.OrdinalIgnoreCase)),
            message: $"Expected only 1 forward to {nonceFwd1}");
        Assert.AreEqual(
            1,
            allForwards.Emails.Count(e => string.Equals(e.ForwardTo, nonceFwd2, StringComparison.OrdinalIgnoreCase)),
            message: $"Expected only 1 forward to {nonceFwd2}");
    }
}
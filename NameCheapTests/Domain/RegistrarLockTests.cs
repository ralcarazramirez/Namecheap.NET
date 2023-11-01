namespace NameCheapTests.Domain;

/// <summary>
/// Tests the registrar lock methods.
/// </summary>
/// <remarks>
/// As the tests ultimately affect external APIs, it's difficult to set up
/// isolated, repeatable tests. As such, we constructed a single test
/// that explores all scenarios based on an initial state.
/// </remarks>
[TestClass]
public class RegistrarLockTests : TestBase
{
    [TestMethod]
    public async Task CanLockAndUnlockTheRegistrarStatus()
    {
        var isLocked = await _api.Domains.GetRegistrarLock(_domainName);

        if (isLocked)
        {
            TestUnlock();
            TestLock();
        }
        else
        {
            TestLock();
            TestUnlock();
        }
    }

    private async Task TestUnlock()
    {
        _api.Domains.SetRegistrarUnlock(_domainName);
        var isLocked = await _api.Domains.GetRegistrarLock(_domainName);
        Assert.IsFalse(isLocked, "Expected registrar lock status to be unlocked because the test just unlocked it.");
    }

    private async Task TestLock()
    {
        _api.Domains.SetRegistrarLock(_domainName);
        var isLocked = await _api.Domains.GetRegistrarLock(_domainName);
        Assert.IsTrue(isLocked, "Expected registrar lock status to be locked because the test just locked it.");
    }
}
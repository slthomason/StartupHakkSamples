using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NumberBadger.Test;

[TestClass]
public class Int64ConversionTest
{
    [TestMethod]
    public void TestEmptyInt64Parsing()
    {
        // Try once with a prefix
        var originalData = 0L;
        var badge = Badger.CreateBadge(originalData, "My");
        Assert.AreEqual("Myrrrrrrrr", badge);
        var result = Badger.ParseInt64(badge, "My");
        Assert.IsTrue(result.Success);
        Assert.AreEqual(result.Value, originalData);
        
        // Now same thing without a prefix
        badge = Badger.CreateBadge(originalData, null);
        Assert.AreEqual("rrrrrrrr", badge);
        result = Badger.ParseInt64(badge, null);
        Assert.IsTrue(result.Success);
        Assert.AreEqual(result.Value, originalData);
    }
    
    [TestMethod]
    public void TestRandomNumbers()
    {
        // Randomly generate a bunch of guids and test them
        var rand = new Random();
        for (var i = 0; i < 1000; i++)
        {
            var originalData = rand.NextInt64();
            var badge = Badger.CreateBadge(originalData, "My");
            var result = Badger.ParseInt64(badge, "My");
            Assert.IsTrue(result.Success);
            Assert.AreEqual(result.Value, originalData);
        }
    }

    [TestMethod]
    public void TestGuidFailures()
    {
        // Try a bad prefix
        var badge = "Something that doesn't start with the correct prefix";
        var result = Badger.ParseInt64(badge, "My");
        Assert.IsFalse(result.Success);
        Assert.AreEqual("The ID 'Something that doesn't start with the correct prefix' does not begin with the correct prefix 'My'.", result.Message);
        
        // Try something with a good prefix but bad data
        badge = "My==========";
        result = Badger.ParseInt64(badge, "My");
        Assert.IsFalse(result.Success);
        Assert.AreEqual("The ID 'My==========' is not a valid identity code.", result.Message);
        
        // Try an incomplete code - I've deleted half the data
        badge = "My1111";
        result = Badger.ParseInt64(badge, "My");
        Assert.IsFalse(result.Success);
        Assert.AreEqual("The ID 'My1111' is incomplete. Did you forget a few characters?", result.Message);
    }
}
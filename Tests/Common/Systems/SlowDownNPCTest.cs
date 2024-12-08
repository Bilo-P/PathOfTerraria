using JetBrains.Annotations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathOfTerraria.Common.Systems;

namespace PathOfTerraria.Tests.Common.Systems;

[TestClass]
[TestSubject(typeof(SlowDownNPC))]
public class SlowDownNPCTest
{
    
    public class TestDoubleNPC : NPC
    {
    }

    [TestMethod]
    public void ResetEffects_SetsSlowDownToZero()
    {
        var testNpc = new TestDoubleNPC
        {
            velocity = new Vector2(10, 0),
            position = new Vector2(50, 50)
        };
        var slowDownNPC = new SlowDownNPC();

        slowDownNPC.ResetEffects(testNpc);

        Assert.AreEqual(0, slowDownNPC.SlowDown);
    }

    [TestMethod]
    public void AI_ClampsSlowDownAndUpdatesPosition()
    {
        var testNpc = new TestDoubleNPC
        {
            velocity = new Vector2(10, 0),
            position = new Vector2(50, 50)
        };

        var slowDownNPC = new SlowDownNPC
        {
            SlowDown = 1.5f // Exceeds valid range
        };

        slowDownNPC.AI(testNpc);

        Assert.AreEqual(1, slowDownNPC.SlowDown, "SlowDown should be clamped to 1");
        Assert.AreEqual(new Vector2(40, 50), testNpc.position, "Position should be updated based on velocity and SlowDown");
        Console.WriteLine($"Position: {testNpc.position}");
    }

    [TestMethod]
    public void AI_ZeroSlowDown_DoesNotChangePosition()
    {
        var testNpc = new TestDoubleNPC
        {
            velocity = new Vector2(10, 0),
            position = new Vector2(50, 50)
        };

        var slowDownNPC = new SlowDownNPC
        {
            SlowDown = 0
        };

        slowDownNPC.AI(testNpc);

        Assert.AreEqual(new Vector2(50, 50), testNpc.position, "Position should remain unchanged when SlowDown is 0");
    }
}

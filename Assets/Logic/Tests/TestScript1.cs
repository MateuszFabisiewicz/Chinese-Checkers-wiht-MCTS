using System.Collections;
using System.Collections.Generic;
using Assets.Logic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
//using Assets.Logic;

public class TestScript1
{
    // A Test behaves as an ordinary method
    [Test]
    public void TestScript1SimplePasses()
    {
        // Use the Assert class to test conditions
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        Assert.AreEqual (testGame.players.Length, 2);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator TestScript1WithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}

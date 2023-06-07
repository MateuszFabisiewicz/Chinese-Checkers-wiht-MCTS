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
    public void CreatingGame()
    {
        // Use the Assert class to test conditions
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        Assert.AreEqual (testGame.players.Length, 2);
    }

    [Test]
    public void FirstMoveTest ()
    {
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        var answer = testGame.players[0].MakeChoice (testGame.board, testGame.players[1]);

        Assert.AreEqual (answer.player, testGame.players[0].color);
    }

    [Test]
    public void TwoUCTGameTest () // TODO: test dwóch graczy UCT, aż któryś nie wygra (albo 100 ruchów wykonanych)
    {
        int i = 0, currPlayer = 0;
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        while (testGame.Win () == PlayerColor.None && i < 100)
        {
            i++;
            var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[1]);
            testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
        }

        //var answer = testGame.players[0].MakeChoice (testGame.board, testGame.players[1]);

        if (i < 100)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else 
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
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

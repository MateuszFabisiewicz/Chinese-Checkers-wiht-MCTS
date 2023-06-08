using System.Collections;
using System.Collections.Generic;
using Assets.Logic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestScript1
{
    [Test]
    public void CreatingGame()
    {
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        Assert.AreEqual (testGame.players.Length, 2);
    }

    [Test]
    public void WinConditionTest ()
    {
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        testGame.MoveChecker (testGame.board.fields[0, 8], 0, 1);
        testGame.MoveChecker (testGame.board.fields[1, 8], 1, 1);
        testGame.MoveChecker (testGame.board.fields[2, 8], 2, 1);
        testGame.MoveChecker (testGame.board.fields[3, 8], 3, 1);
        testGame.MoveChecker (testGame.board.fields[4, 8], 4, 1);
        testGame.MoveChecker (testGame.board.fields[1, 7], 5, 1);
        testGame.MoveChecker (testGame.board.fields[2, 7], 6, 1);
        testGame.MoveChecker (testGame.board.fields[3, 7], 7, 1);
        testGame.MoveChecker (testGame.board.fields[4, 7], 8, 1);
        testGame.MoveChecker (testGame.board.fields[5, 7], 9, 1);

        testGame.MoveChecker (testGame.board.fields[5, 8], 0, 0);
        testGame.MoveChecker (testGame.board.fields[6, 8], 1, 0);
        testGame.MoveChecker (testGame.board.fields[7, 8], 2, 0);
        testGame.MoveChecker (testGame.board.fields[8, 8], 3, 0);
        testGame.MoveChecker (testGame.board.fields[6, 7], 4, 0);
        testGame.MoveChecker (testGame.board.fields[7, 7], 5, 0);
        testGame.MoveChecker (testGame.board.fields[8, 7], 6, 0);
        testGame.MoveChecker (testGame.board.fields[7, 6], 7, 0);
        testGame.MoveChecker (testGame.board.fields[8, 6], 8, 0);
        testGame.MoveChecker (testGame.board.fields[8, 5], 9, 0);

        Assert.AreEqual (testGame.Win (), testGame.players[0].color);
    }

    [Test]
    public void OtherWinConditionTest ()
    {
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        testGame.MoveChecker (testGame.board.fields[0, 8], 0, 1);
        testGame.MoveChecker (testGame.board.fields[1, 8], 1, 1);
        testGame.MoveChecker (testGame.board.fields[2, 8], 2, 1);
        testGame.MoveChecker (testGame.board.fields[3, 8], 3, 1);
        testGame.MoveChecker (testGame.board.fields[4, 8], 4, 1);
        testGame.MoveChecker (testGame.board.fields[1, 7], 5, 1);
        testGame.MoveChecker (testGame.board.fields[2, 7], 6, 1);
        testGame.MoveChecker (testGame.board.fields[3, 7], 7, 1);
        testGame.MoveChecker (testGame.board.fields[4, 7], 8, 1);
        testGame.MoveChecker (testGame.board.fields[5, 7], 9, 1);

        testGame.MoveChecker (testGame.board.fields[5, 8], 0, 0);
        testGame.MoveChecker (testGame.board.fields[6, 8], 1, 0);
        testGame.MoveChecker (testGame.board.fields[7, 8], 2, 0);
        testGame.MoveChecker (testGame.board.fields[8, 8], 3, 0);
        testGame.MoveChecker (testGame.board.fields[6, 7], 4, 0);
        testGame.MoveChecker (testGame.board.fields[7, 7], 5, 0);
        testGame.MoveChecker (testGame.board.fields[8, 7], 6, 0);
        testGame.MoveChecker (testGame.board.fields[7, 6], 7, 0);
        testGame.MoveChecker (testGame.board.fields[8, 6], 8, 0);
        testGame.MoveChecker (testGame.board.fields[8, 5], 9, 0);

        Assert.AreEqual (testGame.board.IsWinning (testGame.players[0].color), true);
    }

    [Test]
    public void FirstMoveTest ()
    {
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        var answer = testGame.players[0].MakeChoice (testGame.board, testGame.players[1]);

        Assert.AreEqual (answer.player, testGame.players[0].color);
    }

    [Test]
    public void TwoUCTGameTest ()
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

        if (i < 100)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else 
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }
}

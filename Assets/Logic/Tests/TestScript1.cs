using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Logic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Linq;

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

        Assert.AreEqual (testGame.board.IsWinning (testGame.players[0].color), 1.0);
    }

    [Test]
    public void FirstUCTMoveTest ()
    {
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        var answer = testGame.players[0].MakeChoice (testGame.board, testGame.players[1]);

        Assert.AreEqual (answer.player, testGame.players[0].color);
    }

    [Test]
    public void TwoUCTGameTest ()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game (PlayerType.UCT, PlayerType.UCT);

        while (testGame.Win () == PlayerColor.None && i < 100)
        {
            i++;
            var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[oppPlayer]);
            testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }

        if (i < 100)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else 
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }

    [Test]
    public void FirstAUCTMoveTest ()
    {
        Game testGame = new Game (PlayerType.AUCT, PlayerType.UCT);

        var answer = testGame.players[0].MakeChoice (testGame.board, testGame.players[1]);

        Assert.AreEqual (answer.player, testGame.players[0].color);
    }

    [Test]
    public void TwoAUCTGameTest ()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game (PlayerType.AUCT, PlayerType.AUCT);

        while (testGame.Win () == PlayerColor.None && i < 10)
        {
            i++;
            var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[oppPlayer]);
            testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }

        if (i < 10)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }

    [Test]
    public void AUCTAndUCTGameTest ()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game (PlayerType.AUCT, PlayerType.UCT);

        while (testGame.Win () == PlayerColor.None && i < 30)
        {
            i++;
            var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[oppPlayer]);
            testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }

        if (i < 30)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }

    #region RAVEandRAVE
    [Test]
    public void RAVEAndRAVEGameTest1()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 1);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest2()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 2);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest3()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 3);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest4()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 4);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest5()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 5);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest6()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 6);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest7()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 7);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest8()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 8);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest9()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 9);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndRAVEGameTest10()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.RAVE, 10);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    #endregion
    #region RAVEandAUCT
    [Test]
    public void RAVEAndAUCTGameTest1()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 1);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        TimeSpan ts2 = new();
        while (testGame.Win() == PlayerColor.None && i < 100)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            else
                ts2 = ts2.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        ts2 = ts2.Divide(i / 2);
        UnityEngine.Debug.Log(ts);
        UnityEngine.Debug.Log(ts2);
        if (i < 100)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest2()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 2);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest3()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 3);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest4()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 4);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest5()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 5);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest6()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 6);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest7()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 7);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest8()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 8);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest9()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 9);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndAUCTGameTest10()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.AUCT, 10);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    #endregion
    #region RAVEandUCT
    [Test]
    public void RAVEAndUCTGameTest1()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT,1);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        TimeSpan ts2 = new();
        while (testGame.Win() == PlayerColor.None && i < 100)
        {
            i++;
            stopwatch.Start ();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop ();
            if (currPlayer == 0)
               ts = ts.Add(stopwatch.Elapsed);
            else
                ts2 = ts2.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i/2);
        ts2 = ts2.Divide(i/2);
        UnityEngine.Debug.Log(ts);
        UnityEngine.Debug.Log(ts2);
        if (i < 100)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest2()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 2);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest3()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 3);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest4()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 4);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest5()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 5);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest6()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 6);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest7()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 7);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest8()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 8);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest9()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 9);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndUCTGameTest10()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT, 10);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    #endregion
    [Test]
    public void RAVEAndHeuristicGameTest1()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 1);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest2()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 2);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest3()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 3);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest4()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 4);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest5()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 5);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest6()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 6);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest7()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 7);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest8()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 8);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest9()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 9);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }
    [Test]
    public void RAVEAndHeuristicGameTest10()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.Heuristic, 10);
        Stopwatch stopwatch = new();
        TimeSpan ts = new();
        while (testGame.Win() == PlayerColor.None && i < 200)
        {
            i++;
            stopwatch.Start();
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[oppPlayer]);
            stopwatch.Stop();
            if (currPlayer == 0)
                ts = ts.Add(stopwatch.Elapsed);
            stopwatch.Reset();
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }
        ts = ts.Divide(i / 2);
        UnityEngine.Debug.Log(ts);

        if (i < 200)
        {
            PlayerColor winner = testGame.Win();
            UnityEngine.Debug.Log(winner);
            Assert.AreNotEqual(winner, PlayerColor.None);
        }
        else
            Assert.AreEqual(testGame.Win(), PlayerColor.None);
    }

    [Test]
    public void FirstHeuristicMoveTest ()
    {
        Game testGame = new Game (PlayerType.Heuristic, PlayerType.UCT);

        var answer = testGame.players[0].MakeChoice (testGame.board, testGame.players[1]);

        Assert.AreEqual (answer.player, testGame.players[0].color);
    }

    [Test]
    public void TwoHeuristicGameTest ()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game (PlayerType.Heuristic, PlayerType.Heuristic);

        while (testGame.Win () == PlayerColor.None && i < 100)
        {
            i++;
            var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[oppPlayer]);
            testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
            oppPlayer = (oppPlayer + 1) % 2;
        }

        if (i < 100)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }

    [Test]
    public void TimingAUCTAndHeuristicGameTest ()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game (PlayerType.AUCT, PlayerType.Heuristic);
        Stopwatch auctWatch = new Stopwatch ();
        Stopwatch heuristicWatch = new Stopwatch ();
        int sumAUCT = 0, sumHeuristic = 0;
        double[] avgAuct = new double[10];
        double[] avgHeur = new double[10];

        for (int j = 0; j < 10; j++)
        {
            i = 0;
            currPlayer = 0;
            oppPlayer = 1;

            while (testGame.Win () == PlayerColor.None && i < 201)
            {
                i++;
                if (currPlayer == 0)
                {
                    auctWatch.Start ();
                }
                else
                {
                    heuristicWatch.Start ();
                }
                var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[oppPlayer]);
                testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
                currPlayer = (currPlayer + 1) % 2;
                oppPlayer = (oppPlayer + 1) % 2;

                if (currPlayer == 0) // poprzedni był 1, czyli Heurystyka
                {
                    heuristicWatch.Stop ();
                    sumHeuristic += (int)heuristicWatch.ElapsedMilliseconds;
                    heuristicWatch.Reset ();
                }
                else
                {
                    auctWatch.Stop ();
                    sumAUCT += (int)auctWatch.ElapsedMilliseconds;
                    auctWatch.Reset ();
                }
            }
            avgAuct[j] = sumAUCT / (i / 2); // plus minus jest to średnia
            avgHeur[j] = sumHeuristic / (i / 2);

            sumAUCT = 0;
            sumHeuristic = 0;
        }

        double avgAUCTTime = avgAuct.Average ();
        double avgHeuristicTime = avgHeur.Average ();


        if (i < 201)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }

    [Test]
    public void TimingAUCTAndUCTGameTest ()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game (PlayerType.AUCT, PlayerType.UCT);
        Stopwatch auctWatch = new Stopwatch ();
        Stopwatch uctWatch = new Stopwatch ();
        int sumAUCT = 0, sumUct = 0;
        double[] avgAuct = new double[10];
        double[] avgUct = new double[10];

        for (int j = 0; j < 10; j++)
        {
            i = 0;
            currPlayer = 0;
            oppPlayer = 1;

            while (testGame.Win () == PlayerColor.None && i < 201)
            {
                i++;
                if (currPlayer == 0)
                {
                    auctWatch.Start ();
                }
                else
                {
                    uctWatch.Start ();
                }
                var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[oppPlayer]);
                testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
                currPlayer = (currPlayer + 1) % 2;
                oppPlayer = (oppPlayer + 1) % 2;

                if (currPlayer == 0) // poprzedni był 1, czyli Heurystyka
                {
                    uctWatch.Stop ();
                    sumUct += (int)uctWatch.ElapsedMilliseconds;
                    uctWatch.Reset ();
                }
                else
                {
                    auctWatch.Stop ();
                    sumAUCT += (int)auctWatch.ElapsedMilliseconds;
                    auctWatch.Reset ();
                }
            }
            avgAuct[j] = sumAUCT / (i / 2); // plus minus jest to średnia
            avgUct[j] = sumUct / (i / 2);

            sumAUCT = 0;
            sumUct = 0;
        }

        double avgAUCTTime = avgAuct.Average ();
        double avgUctTime = avgUct.Average ();


        if (i < 201)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }

    [Test]
    public void TimingHeurAndUCTGameTest ()
    {
        int i = 0, currPlayer = 0, oppPlayer = 1;
        Game testGame = new Game (PlayerType.Heuristic, PlayerType.UCT);
        Stopwatch heurWatch = new Stopwatch ();
        Stopwatch uctWatch = new Stopwatch ();
        int sumHeur = 0, sumUct = 0;
        double[] avgHeur = new double[10];
        double[] avgUct = new double[10];

        for (int j = 0; j < 10; j++)
        {
            i = 0;
            currPlayer = 0;
            oppPlayer = 1;

            while (testGame.Win () == PlayerColor.None && i < 201)
            {
                i++;
                if (currPlayer == 0)
                {
                    heurWatch.Start ();
                }
                else
                {
                    uctWatch.Start ();
                }
                var answer = testGame.players[currPlayer].MakeChoice (testGame.board, testGame.players[oppPlayer]);
                testGame.MoveChecker (answer.newField, answer.checkerIndex, currPlayer);
                currPlayer = (currPlayer + 1) % 2;
                oppPlayer = (oppPlayer + 1) % 2;

                if (currPlayer == 0) // poprzedni był 1, czyli Heurystyka
                {
                    uctWatch.Stop ();
                    sumUct += (int)uctWatch.ElapsedMilliseconds;
                    uctWatch.Reset ();
                }
                else
                {
                    heurWatch.Stop ();
                    sumHeur += (int)heurWatch.ElapsedMilliseconds;
                    heurWatch.Reset ();
                }
            }
            avgHeur[j] = sumHeur / (i / 2); // plus minus jest to średnia
            avgUct[j] = sumUct / (i / 2);

            sumHeur = 0;
            sumUct = 0;
        }

        double avgAUCTTime = avgHeur.Average ();
        double avgUctTime = avgUct.Average ();


        if (i < 201)
            Assert.AreNotEqual (testGame.Win (), PlayerColor.None);
        else
            Assert.AreEqual (testGame.Win (), PlayerColor.None);
    }
}

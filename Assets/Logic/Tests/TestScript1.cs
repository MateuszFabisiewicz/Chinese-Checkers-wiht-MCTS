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

    [Test]
    public void RAVEAndUCTGameTest()
    {
        int i = 0, currPlayer = 0;
        Game testGame = new Game(PlayerType.RAVE, PlayerType.UCT);

        while (testGame.Win() == PlayerColor.None && i < 30)
        {
            i++;
            var answer = testGame.players[currPlayer].MakeChoice(testGame.board, testGame.players[1]);
            testGame.MoveChecker(answer.newField, answer.checkerIndex, currPlayer);
            currPlayer = (currPlayer + 1) % 2;
        }

        if (i < 30)
            Assert.AreNotEqual(testGame.Win(), PlayerColor.None);
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

using Assets.Logic;
using Assets.Logic.Algorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player //: MonoBehaviour
{
    public int id;
    public PlayerColor color;
    protected PlayerType type;
    public Checker[] checkers;
    protected Node root;

    public Player (PlayerColor color)
    {
        this.color = color;
        checkers = new Checker[Game.checkerCount];

        for (int i = 0; i < Game.checkerCount; i++)
        {
            checkers[i] = new Checker (i, color);
        }
    }

    public void MakeMove (int checkerIndex, FieldInBoard end)
    {
        checkers[checkerIndex].SetPosition (end);
    }

    public abstract (FieldInBoard, int) MakeChoice (Board board, Player opponentStats);
}

public class HumanPlayer : Player
{
    public HumanPlayer (PlayerColor color) : base(color)
    {
        type = PlayerType.Human;
    }

    public override (FieldInBoard, int) MakeChoice (Board board, Player opponentStats)
    {
        throw new System.NotImplementedException ();
    }
}

public class RAVEPlayer : Player
{
    public RAVEPlayer (PlayerColor color) : base (color)
    {
        type = PlayerType.RAVE;
    }

    public override (FieldInBoard, int) MakeChoice (Board board, Player opponentStats)
    {
        throw new System.NotImplementedException ();
    }
}

public class AUCTPlayer : Player
{
    public AUCTPlayer (PlayerColor color) : base (color)
    {
        type = PlayerType.AUCT;
    }

    public override (FieldInBoard, int) MakeChoice (Board board, Player opponentStats)
    {
        throw new System.NotImplementedException ();
    }
}

public class HeurPlayer : Player
{
    public HeurPlayer (PlayerColor color) : base (color)
    {
        type = PlayerType.Heuristic;
    }

    public override (FieldInBoard, int) MakeChoice (Board board, Player opponentStats)
    {
        throw new System.NotImplementedException ();
    }
}

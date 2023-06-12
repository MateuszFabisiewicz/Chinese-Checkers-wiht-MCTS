using Assets.Logic;
using Assets.Logic.Algorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player
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

    //public void MakeMove (int checkerIndex, FieldInBoard end)
    //{
        //checkers[checkerIndex].SetPosition (end);
    //}

    public abstract (FieldInBoard newField, int checkerIndex, PlayerColor player) MakeChoice (Board board, Player opponentStats);
}

public class HumanPlayer : Player
{
    public HumanPlayer (PlayerColor color) : base(color)
    {
        type = PlayerType.Human;
    }

    public override (FieldInBoard, int, PlayerColor) MakeChoice (Board board, Player opponentStats)
    {
        throw new System.NotImplementedException ();
    }
}


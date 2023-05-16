using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public const int side = 9; // liczba pól na boku
    //const int playerCount = 2; // liczba graczy
    //const int checkerCount = 10; // liczba pionków gracza
    public const int triangleSide = 4; // wielkość boku trójkąta gracza
    public FieldInBoard[,] fields = new FieldInBoard[side, side];
    //public Checker[,] checkers = new Checker[playerCount, checkerCount];

    public Board ()
    {
        int checkerCounter = 0;
        for (int i = 0; i < side; i++)
        {
            for (int j = 0; j < side; j++)
            {
                FieldInBoard newField = new FieldInBoard
                {
                    x = i,
                    y = j
                };
                //Checker newChecker = null;

                if (i < triangleSide && j < triangleSide)
                {
                    newField.fieldType = PlayerColor.Blue;
                    newField.playerOnField = PlayerColor.Blue;
                    //newChecker = new Checker (checkerCounter, PlayerColor.Blue);
                    checkerCounter++;
                }
                else if (i >= side- triangleSide && j >= side- triangleSide)
                {
                    newField.fieldType = PlayerColor.Red;
                    newField.playerOnField = PlayerColor.Red;
                    //newChecker = new Checker (checkerCounter, PlayerColor.Red);
                }
                else
                {
                    newField.fieldType = PlayerColor.None;
                    newField.playerOnField = PlayerColor.None;
                }

                fields[i, j] = newField;
                //if (newChecker != null)
                //{
                //    newChecker.SetPosition (fields[i, j]);
                //}
            }
        }
    }
}

public class Checker
{
    public int ID { get; private set; }
    public PlayerColor color { get; private set; }
    public FieldInBoard placement { get; private set; }

    public Checker (int id, PlayerColor color)
    {
        ID = id;
        this.color = color;
    }

    public void SetPosition (FieldInBoard newPlace)
    {
        placement = newPlace;
    }
}

public struct FieldInBoard
{
    public int x;
    public int y;
    public PlayerColor fieldType;
    public PlayerColor playerOnField;
}

public enum PlayerColor
{
    None,
    Red,
    Blue
}
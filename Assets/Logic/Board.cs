using Assets.Logic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board // może być interpretowana jak stan gry
{
    public const int side = 9; // liczba pól na boku
    public const int triangleSide = 4; // wielkość boku trójkąta gracza
    public FieldInBoard[,] fields = new FieldInBoard[side, side];

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

                if (i < triangleSide && j < triangleSide)
                {
                    newField.fieldType = PlayerColor.Blue;
                    newField.playerOnField = PlayerColor.Blue;
                    checkerCounter++;
                }
                else if (i >= side- triangleSide && j >= side- triangleSide)
                {
                    newField.fieldType = PlayerColor.Red;
                    newField.playerOnField = PlayerColor.Red;
                }
                else
                {
                    newField.fieldType = PlayerColor.None;
                    newField.playerOnField = PlayerColor.None;
                }

                fields[i, j] = newField;
            }
        }
    }

    public FieldInBoard FindCheckersPosition (int checker, PlayerColor color)
    {
        for (int i = 0; i < side; i++)
        {
            for (int j = 0; j < side; j++)
            {
                if (fields[i, j].playerOnField == color && fields[i, j].checkerIndex == checker)
                {
                    return fields[i, j];
                }
            }
        }

        throw new System.Exception ("Nie znaleziono pionka o podanym indeksie i kolorze");
    }
}

public struct FieldInBoard
{
    public int x;
    public int y;
    public PlayerColor fieldType;
    public PlayerColor playerOnField;
    public int checkerIndex;
}
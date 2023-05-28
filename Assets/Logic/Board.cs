using Assets.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

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

    public Board(Board oldBoard)
    {
        for (int i = 0; i < side; i++)
        {
            for (int j = 0; j < side; j++)
            {
                FieldInBoard newField = new FieldInBoard
                {
                    x = i,
                    y = j
                };

                newField.fieldType = oldBoard.fields[i, j].fieldType;
                newField.playerOnField = oldBoard.fields[i, j].playerOnField;
                newField.checkerIndex = oldBoard.fields[i, j].checkerIndex;
                newField.checker = new Checker (oldBoard.fields[i, j].checker);

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

    internal bool IsWinning (PlayerColor color)
    {
        // jeśli gracz wypełnił cały trójkąt swoimi pionkami, to wygrał
        // wpp uznajemy że przegrał 

        int playerCounter = 0, opponentCounter = 0;
        for (int i = 0; i < triangleSide; i++)
        {
            for (int j = 0; j < triangleSide; j++)
            {
                // if (fields[i, j].fieldType != PlayerColor.None) -- tylko takie rozważamy
                if (fields[i, j].playerOnField != PlayerColor.None)
                {
                    if (fields[i, j].playerOnField == color && fields[i, j].fieldType != color)
                    {
                        playerCounter++;
                    }
                    else if (fields[i, j].playerOnField != color && fields[i, j].fieldType == color)
                    {
                        opponentCounter++;
                    }
                }

                if (fields[side - i - 1, side - j - 1].playerOnField != PlayerColor.None)
                {
                    if (fields[side - i - 1, side - j - 1].playerOnField == color && fields[side - i - 1, side - j - 1].fieldType != color)
                    {
                        playerCounter++;
                    }
                    else if (fields[side - i - 1, side - j - 1].playerOnField != color && fields[side - i - 1, side - j - 1].fieldType == color)
                    {
                        opponentCounter++;
                    }
                }
            }
        }

        if (playerCounter == Game.checkerCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Board(Board oldBoard, FieldInBoard oldField, FieldInBoard newField)
    {
        Board newBoard = new Board (oldBoard);

        newBoard.fields[newField.x, newField.y].checkerIndex = oldField.checkerIndex;
        newBoard.fields[newField.x, newField.y].checker = new Checker (oldField.checker);
        newBoard.fields[newField.x, newField.y].playerOnField = oldField.playerOnField;

        oldBoard.fields[oldField.x, oldField.y].playerOnField = PlayerColor.None;
        oldBoard.fields[oldField.x, oldField.y].checkerIndex = -1;
        oldBoard.fields[oldField.x, oldField.y].checker = null;
    }
}

public struct FieldInBoard
{
    public int x;
    public int y;
    public PlayerColor fieldType;
    public PlayerColor playerOnField;
    public int checkerIndex;
    public Checker checker;
}
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
        for (int i = 0; i < side; i++)
        {
            for (int j = 0; j < side; j++)
            {
                FieldInBoard newField = new FieldInBoard
                {
                    x = i,
                    y = j
                };

                if (i < triangleSide && j < triangleSide - i)
                {
                    newField.fieldType = PlayerColor.Blue;
                    newField.playerOnField = PlayerColor.Blue;
                }
                else if (i >= side - triangleSide && j >= 2*side - triangleSide - i - 1)
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
                newField.checker = oldBoard.fields[i, j].checker != null ? oldBoard.fields[i, j].checker : null;
                if (newField.checker != null) // potrzebne??
                {
                    newField.checker.SetPosition (newField);
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
                if (fields[i, j].playerOnField == color && fields[i, j].checker.ID == checker)
                {
                    return fields[i, j];
                }
            }
        }

        throw new System.Exception ("Nie znaleziono pionka o podanym indeksie i kolorze");
    }

    public double IsWinning (PlayerColor color)
    {
        // jeśli gracz wypełnił cały trójkąt swoimi pionkami, to wygrał
        // jeśli drugi gracz wypełnił to uznajemy że przegrał 
        // wpp liczymy "stopień wygrania" - ile swoich pionków ma gracz na polach przeciwnika

        int playerCounter = 0, opponentCounter = 0, playerMovedOut = 0;

        for (int i = 0; i < side; i++)
        {
            for (int j = 0; j < side; j++)
            {
                if (fields[i, j].playerOnField != PlayerColor.None)
                {
                    if (fields[i, j].playerOnField == color && fields[i, j].fieldType != color)
                    {
                        playerCounter++;
                    }
                    else if (fields[i, j].playerOnField != color && fields[i,j].playerOnField != PlayerColor.None && fields[i, j].fieldType == color)
                    {
                        opponentCounter++;
                    }
                }
                else if (fields[i, j].playerOnField == color)
                {
                    playerMovedOut++;
                }
            }
        }

        //for (int i = 0; i < triangleSide; i++)
        //{
        //    for (int j = 0; j < triangleSide; j++)
        //    {
        //        // if (fields[i, j].fieldType != PlayerColor.None) -- tylko takie rozważamy
        //        if (fields[i, j].playerOnField != PlayerColor.None)
        //        {
        //            if (fields[i, j].playerOnField == color && fields[i, j].fieldType != color)
        //            {
        //                playerCounter++;
        //            }
        //            else if (fields[i, j].playerOnField != color && fields[i, j].fieldType == color)
        //            {
        //                opponentCounter++;
        //            }
        //        }

        //        if (fields[side - i - 1, side - j - 1].playerOnField != PlayerColor.None)
        //        {
        //            if (fields[side - i - 1, side - j - 1].playerOnField == color && fields[side - i - 1, side - j - 1].fieldType != color)
        //            {
        //                playerCounter++;
        //            }
        //            else if (fields[side - i - 1, side - j - 1].playerOnField != color && fields[side - i - 1, side - j - 1].fieldType == color)
        //            {
        //                opponentCounter++;
        //            }
        //        }
        //    }
        //}

        if (playerCounter == Game.checkerCount)
        {
            return 1;
        }
        else if (opponentCounter == Game.checkerCount)
        {
            return 0;
        }
        else
        {
            // odległość "najwyższego pionka" od przeciwnej strony i też to ile mamy nie ruszony w ogóle

            //if (playerCounter == 0)
            //{
            //    return playerMovedOut / (2 * Game.checkerCount); // żeby dawało mniejsze "wygranie" niż faktyczne dotarcie do trójkąta
            //}

            return (double)playerCounter / (double)Game.checkerCount;
        }
    }

    public Board(Board oldBoard, FieldInBoard oldField, FieldInBoard newField) : this (oldBoard)
    {
        fields[newField.x, newField.y].playerOnField = oldField.playerOnField;
        fields[newField.x, newField.y].checker = new Checker (oldField.checker);
        fields[newField.x, newField.y].checker.SetPosition (fields[newField.x, newField.y]);

        fields[oldField.x, oldField.y].playerOnField = PlayerColor.None;
        fields[oldField.x, oldField.y].checker = null;
    }
}

public struct FieldInBoard
{
    public int x;
    public int y;
    public PlayerColor fieldType;
    public PlayerColor playerOnField;
    public Checker checker;
}
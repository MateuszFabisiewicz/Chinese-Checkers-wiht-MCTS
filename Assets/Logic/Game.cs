using Assets.Logic;
using Assets.Logic.Algorithms;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game
{
    public const int playerCount = 2;
    public const int checkerCount = 10; // liczba pionków gracza

    public Board board;
    public Player[] players = new Player[playerCount];


    public Game (PlayerType playerBlue, PlayerType playerRed,int seed=123)
    {
        // tworzymy graczy
        switch (playerBlue)
        {
            case PlayerType.Human:
                players[0] = new HumanPlayer (PlayerColor.Blue);
                break;

            case PlayerType.UCT:
                players[0] = new UCTPlayer (PlayerColor.Blue,seed);
                break;

            case PlayerType.RAVE:
                players[0] = new RAVEPlayer (PlayerColor.Blue,seed);
                break;

            case PlayerType.AUCT:
                players[0] = new AUCTPlayer (PlayerColor.Blue,seed);
                break;

            case PlayerType.Heuristic:
                players[0] = new HeuristicPlayer (PlayerColor.Blue);
                break;
        }

        switch (playerRed)
        {
            case PlayerType.Human:
                players[1] = new HumanPlayer (PlayerColor.Red);
                break;

            case PlayerType.UCT:
                players[1] = new UCTPlayer (PlayerColor.Red,seed);
                break;

            case PlayerType.RAVE:
                players[1] = new RAVEPlayer (PlayerColor.Red, seed);
                break;

            case PlayerType.AUCT:
                players[1] = new AUCTPlayer (PlayerColor.Red,seed);
                break;

            case PlayerType.Heuristic:
                players[1] = new HeuristicPlayer (PlayerColor.Red);
                break;
        }


        players[0].id = 0;
        players[1].id = 1;

        // tworzymy planszę
        board = new Board ();

        // rozstawiamy pionki graczy
        int checkerIndex = 0;
        for (int i = 0; i < Board.triangleSide; i++)
        {
            for (int j = 0; j < Board.triangleSide - i; j++)
            {
                board.fields[i, j].playerOnField = players[0].color;
                board.fields[i, j].checker = players[0].checkers[checkerIndex];
                players[0].checkers[checkerIndex].SetStartingPosition (board.fields[i, j]);

                board.fields[Board.side - i - 1, Board.side - j - 1].playerOnField = players[1].color;
                board.fields[Board.side - i - 1, Board.side - j - 1].checker = players[1].checkers[checkerIndex];
                players[1].checkers[checkerIndex].SetStartingPosition (board.fields[Board.side - i - 1, Board.side - j - 1]);

                checkerIndex++;
            }
        }
    }

    public PlayerColor Win ()
    {
        int countBlue = 0;
        int countRed = 0;

        for (int i = 0; i < Board.side; i++)
        {
            for (int j = 0; j < Board.side; j++)
            {
                if (board.fields[i, j].playerOnField != PlayerColor.None && board.fields[i, j].fieldType != PlayerColor.None &&
                    board.fields[i, j].playerOnField != board.fields[i, j].fieldType)
                {
                    if (board.fields[i, j].playerOnField == PlayerColor.Blue)
                        countBlue++;
                    else
                        countRed++;
                }
            }
        }

        if (countBlue == checkerCount)
            return PlayerColor.Blue;
        else if (countRed == checkerCount)
            return PlayerColor.Red;
        else
            return PlayerColor.None;
    }

    public void MoveChecker (FieldInBoard newField, int checkerIndex, int playerWhoMoved)
    {
        FieldInBoard oldField = board.FindCheckersPosition (checkerIndex, players[playerWhoMoved].color);

        board.fields[newField.x, newField.y].playerOnField = players[playerWhoMoved].color;
        board.fields[newField.x, newField.y].checker = players[playerWhoMoved].checkers[checkerIndex];

        board.fields[oldField.x, oldField.y].playerOnField = PlayerColor.None;
        board.fields[oldField.x, oldField.y].checker = null;
    }
}

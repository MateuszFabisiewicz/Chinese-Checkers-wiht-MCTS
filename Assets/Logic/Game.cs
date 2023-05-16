using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game //: MonoBehaviour
{
    public const int playerCount = 2;
    public const int checkerCount = 10; // liczba pionków gracza

    public Board board;
    public Player[] players = new Player[playerCount];


    public Game (PlayerType playerBlue, PlayerType playerRed)
    {
        // tworzymy graczy
        switch (playerBlue)
        {
            case PlayerType.Human:
                players[0] = new HumanPlayer (PlayerColor.Blue);
                break;

            case PlayerType.UCT:
                players[0] = new UCTPlayer (PlayerColor.Blue);
                break;

            case PlayerType.RAVE:
                players[0] = new RAVEPlayer (PlayerColor.Blue);
                break;

            case PlayerType.AUCT:
                players[0] = new AUCTPlayer (PlayerColor.Blue);
                break;

            case PlayerType.Heuristic:
                players[0] = new HeurPlayer (PlayerColor.Blue);
                break;
        }

        switch (playerRed)
        {
            case PlayerType.Human:
                players[1] = new HumanPlayer (PlayerColor.Red);
                break;

            case PlayerType.UCT:
                players[1] = new UCTPlayer (PlayerColor.Red);
                break;

            case PlayerType.RAVE:
                players[1] = new RAVEPlayer (PlayerColor.Red);
                break;

            case PlayerType.AUCT:
                players[1] = new AUCTPlayer (PlayerColor.Red);
                break;

            case PlayerType.Heuristic:
                players[1] = new HeurPlayer (PlayerColor.Red);
                break;
        }

        // tworzymy planszę
        board = new Board ();

        // rozstawiamy pionki graczy
        int checkerIndex = 0;
        for (int i = 0; i < Board.triangleSide; i++)
        {
            for (int j = 0; j < Board.triangleSide; j++)
            {
                players[0].checkers[checkerIndex].SetPosition (board.fields[i, j]);
                players[1].checkers[checkerIndex].SetPosition (board.fields[Board.side - i - 1, Board.side - j - 1]);
                checkerIndex++;
            }
        }
    }

    public void Start ()
    {

    }
}

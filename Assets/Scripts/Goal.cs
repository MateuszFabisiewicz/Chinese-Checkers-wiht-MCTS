using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private GameManager gameManager;
    public Field field;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {
        if (gameManager.isPawnClicked && transform.GetComponent<Renderer>().material.color == Color.green)
        {
            gameManager.goalPosition = transform.position;
            gameManager.goalField = field;
            gameManager.canMove = true;

            gameManager.game.MoveChecker (gameManager.game.board.fields[gameManager.goalField.mctsX, gameManager.goalField.mctsY],
                gameManager.game.board.fields[gameManager.startField.mctsX, gameManager.startField.mctsY].checker.ID, gameManager.playerMoving);
        }
    }
}

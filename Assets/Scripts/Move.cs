using Assets.Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Move : MonoBehaviour
{
    private GameManager gameManager;
    private int speed = 5;
    public Field field;
    public PlayerColor playerColor;
    public bool firstClick = false;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (firstClick && gameManager.canMove)
        {
            if(Vector2.Distance(transform.position, gameManager.goalPosition) < 0.001)
            {
                gameManager.canMove = false; 
                gameManager.ClearHighlihtOnBoard();
                
                firstClick = !firstClick;
                gameManager.isPawnClicked = false;

                //game.board.fields[startField.mctsX, startField.mctsY].checker.ID
                //gameManager.movedCheckerId = gameManager.game.board.fields[field.mctsX, field.mctsY].checker.ID;

                int index = Array.FindIndex(gameManager.player1Pawns,x => x.Equals(field));
                if(index > 0)
                    gameManager.player1Pawns[index] = gameManager.goalField;

                index = Array.FindIndex(gameManager.player2Pawns, x => x.Equals(field));
                if (index > 0)
                    gameManager.player2Pawns[index] = gameManager.goalField;
                
                field = gameManager.goalField;
            }
            
            var step = speed * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, gameManager.goalPosition, step);
            
        }
    }
    private void OnMouseDown()
    {
        if (!gameManager.isPawnClicked || firstClick)
        {
            firstClick = !firstClick;
            if (firstClick)
            {
                if (gameManager.HighlightPossibleMoveFields (field, playerColor))
                { 
                    gameManager.isPawnClicked = true;
                    //gameManager.movedCheckerId = gameManager.game.board.fields[field.mctsX, field.mctsY].checker.ID;
                    gameManager.startField = field;
                }
                else
                    firstClick = false;
            }
            else
            {
                gameManager.ClearHighlihtOnBoard();
                gameManager.isPawnClicked = false;
            }
        }
    }
}

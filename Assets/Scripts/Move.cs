using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    GameManager gameManager;
    public Field field;
    public int type;
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
            var step = 5 * Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, gameManager.goalPosition,step);
        }
    }
    private void OnMouseDown()
    {
        firstClick = !firstClick;
        //Debug.Log(field.id + ";" + field.rowNumber + ";" + field.columnNumber);
        var emptyFields = gameManager.EmptyNeighbourFields(field);
        foreach (var field in emptyFields)
        {
            Debug.Log(field.id+";"+field.rowNumber+";"+field.columnNumber);
            field.prefab.transform.GetComponent<Renderer>().material.color = Color.yellow;
        }
        
    }
}

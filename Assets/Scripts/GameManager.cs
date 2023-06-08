using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Logic;
using System;
using System.Linq;

[System.Serializable]
public struct Field: IEquatable<Field>
{
    public int id;
    public int rowNumber;
    public int columnNumber;
    public GameObject prefab;
    //public List<Field> neighbours;
    // All possible neighbours:
    // (rowNumber - 1, columnNumber)
    // (rowNumber - 1, columnNumber - 1)
    // (rowNumber, columnNumber - 1)
    // (rowNumber, columnNumber + 1)
    // (rowNumber + 1, columnNumber)
    // (rowNumber + 1, columnNumber + 1)

    public bool Equals(Field f) => rowNumber == f.rowNumber && columnNumber == f.columnNumber;
    public override int GetHashCode()
    {
        return id.GetHashCode() ^ rowNumber.GetHashCode() ^ columnNumber.GetHashCode();
    }
}

public class GameManager : MonoBehaviour
{
    private float x = -0.02f;
    private float y = 4.25f;
    private readonly int fieldsNumber = 81;
    private readonly int playerPawnsNumber = 10;
    private readonly int numberOfRows = 17;
    private readonly int[] numberOfColumnsInRow = { 1,2,3,4,5,6,7,8,9,8,7,6,5,4,3,2,1 };
    public Field[] fields;
    public Field[] player1Pawns;
    public Field[] player2Pawns;
    public bool canMove = false;
    public Vector2 goalPosition;

    public GameObject player1PawnPrefab;
    public GameObject player2PawnPrefab;
    public GameObject fieldPrefab;

    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
        CreatePlayer1();
        CreatePlayer2();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateBoard()
    {
        fields = new Field[fieldsNumber];
        int index = 0;
        for(int i = 0; i < numberOfRows; i++)
        {
            float firstFieldX = x;
            for(int j = 0; j < numberOfColumnsInRow[i]; j++)
            {
                var field = Instantiate(fieldPrefab, new Vector3(x,y,0), Quaternion.identity);
                fields[index] = new Field()
                {
                    id = index++,
                    rowNumber = i,
                    columnNumber = j,
                    prefab = field
                };
                /*
                var move = field.GetComponent<Move>();
                move.type = 0;
                move.field = new Field()
                {
                    id = index,
                    rowNumber = i,
                    columnNumber = j,
                    prefab = field
                };
                fields[index++] = move.field;
                */
                x +=0.7f;
            }

            x = i >= 8 ? firstFieldX + 0.355f : firstFieldX - 0.355f;
            y -= 0.5f;
        }
    }

    void CreatePlayer1()
    {
        player1Pawns = new Field[playerPawnsNumber];

        for(int i = 0; i < 10; i++)
        {
            GameObject player1 = Instantiate(player1PawnPrefab, fields[i].prefab.transform.position, Quaternion.identity);
            var move = player1.GetComponent<Move>();
            move.type = 1;
            move.field = new Field()
            {
                id = fields[i].id,
                rowNumber = fields[i].rowNumber,
                columnNumber = fields[i].columnNumber,
                prefab = player1
            };
            player1Pawns[i] = move.field;
        }
    }

    void CreatePlayer2()
    {
        player2Pawns = new Field[playerPawnsNumber];
        int j = 0;
        for (int i = fieldsNumber - 1; i >= fieldsNumber - 10; i--)
        {
            GameObject player2 = Instantiate(player2PawnPrefab, fields[i].prefab.transform.position, Quaternion.identity);
            var move = player2.GetComponent<Move>();
            move.type = 2;
            move.field = new Field()
            {
                id = fields[i].id,
                rowNumber = fields[i].rowNumber,
                columnNumber = fields[i].columnNumber,
                prefab = player2
            };
            player2Pawns[j++] = move.field;
        }
    }

    bool CheckNeighbour(Field f,Field potentialNeighbour)
    {
        if (potentialNeighbour.rowNumber == f.rowNumber && (potentialNeighbour.columnNumber == f.columnNumber - 1 || potentialNeighbour.columnNumber == f.columnNumber + 1))
                return true;
        
        if (f.rowNumber < 8)
        {
            if (potentialNeighbour.rowNumber == f.rowNumber - 1 && (potentialNeighbour.columnNumber == f.columnNumber - 1 || potentialNeighbour.columnNumber == f.columnNumber))
                return true;
            if (potentialNeighbour.rowNumber == f.rowNumber + 1 && (potentialNeighbour.columnNumber == f.columnNumber + 1 || potentialNeighbour.columnNumber == f.columnNumber))
                return true;
        }
        else if(f.rowNumber == 8)
        {
            if ((potentialNeighbour.rowNumber == f.rowNumber - 1 || potentialNeighbour.rowNumber == f.rowNumber + 1) && (potentialNeighbour.columnNumber == f.columnNumber - 1 || potentialNeighbour.columnNumber == f.columnNumber))
                return true;
        }
        else
        {
            if (potentialNeighbour.rowNumber == f.rowNumber + 1 && (potentialNeighbour.columnNumber == f.columnNumber - 1 || potentialNeighbour.columnNumber == f.columnNumber))
                return true;
            if (potentialNeighbour.rowNumber == f.rowNumber - 1 && (potentialNeighbour.columnNumber == f.columnNumber + 1 || potentialNeighbour.columnNumber == f.columnNumber))
                return true;
        }

        return false;
    }

    public List<Field> EmptyNeighbourFields(Field f)
    {
        var adjacentPlayer1Pawns = Array.FindAll(player1Pawns, x => CheckNeighbour(f, x));
        var adjacentPlayer2Pawns = Array.FindAll(player2Pawns, x => CheckNeighbour(f, x));
        
        var playersPawns = adjacentPlayer1Pawns.Union(adjacentPlayer2Pawns);
        var adjacentFields = Array.FindAll(fields, x => CheckNeighbour(f,x));

        return adjacentFields.Except(playersPawns).ToList();
    }
}

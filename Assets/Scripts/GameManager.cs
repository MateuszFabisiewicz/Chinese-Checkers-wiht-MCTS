using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Logic;
using System;
using System.Linq;

[System.Serializable]
public class Field: IEquatable<Field>
{
    public int id;
    public int rowNumber;
    public int columnNumber;
    public int mctsX;
    public int mctsY;
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
    public Field goalField;
    public bool isPawnClicked = false;

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
                var goal = field.GetComponent<Goal>();
                goal.field =  new Field()
                {
                    id = index,
                    rowNumber = i,
                    columnNumber = j,
                    prefab = field
                };
                fields[index++] = goal.field;
                x += 0.7f;
            }

            x = i >= 8 ? firstFieldX + 0.355f : firstFieldX - 0.355f;
            y -= 0.5f;
        }
        
        // konwersja wsp�rz�dnych planszy na wsp�rz�dne z Board
        int col = 0;
        int row = 16;
        
        for(int i = 0;i < 9; i++)
        {
            row = 16 - i;
            col = i;
            for (int j = 0; j < 9; j++)
            {
                var pom = fields.FirstOrDefault(x => x.columnNumber == col && x.rowNumber == row);
                if (pom != null)
                {
                    pom.mctsX = i;
                    pom.mctsY = j;
                }
                row--;
                if(row < 8)
                {
                    col--;
                }
            }
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
                mctsX = fields[i].mctsX,
                mctsY = fields[i].mctsY,
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
                mctsX = fields[i].mctsX,
                mctsY = fields[i].mctsY,
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

    private List<Field> EmptyNeighbourFields(Field f)
    {
        var adjacentPlayer1Pawns = Array.FindAll(player1Pawns, x => CheckNeighbour(f, x));
       
        var adjacentPlayer2Pawns = Array.FindAll(player2Pawns, x => CheckNeighbour(f, x));
        
        var playersPawns = adjacentPlayer1Pawns.Union(adjacentPlayer2Pawns);
        
        var adjacentFields = Array.FindAll(fields, x => CheckNeighbour(f,x));
        
        return adjacentFields.Except(playersPawns).ToList();
    }
    
    private List<Field> FieldsWhereCanJump(Field f)
    {
        List<Field> result = new();

        var adjacentPlayer1Pawns = Array.FindAll(player1Pawns, x => CheckNeighbour(f, x));
        var adjacentPlayer2Pawns = Array.FindAll(player2Pawns, x => CheckNeighbour(f, x));
        var adjacentPlayersPawns = adjacentPlayer1Pawns.Union(adjacentPlayer2Pawns);

        foreach(var neighbour in adjacentPlayersPawns)
        {
            var adjacentFields = EmptyNeighbourFields(neighbour);
            result.AddRange(adjacentFields.Where(x => 
            Math.Abs(x.columnNumber - f.columnNumber) == 2 && Math.Abs(x.rowNumber - f.rowNumber) == 2
            || Math.Abs(x.columnNumber - f.columnNumber) == 2 && Math.Abs(x.rowNumber - f.rowNumber) == 0
            || Math.Abs(x.columnNumber - f.columnNumber) == 0 && Math.Abs(x.rowNumber - f.rowNumber) == 2));
        }

        return result;
    }
    
    public void HighlightPossibleMoveFields(Field f,Color c)
    {
        var emptyFields = EmptyNeighbourFields(f);
        var fieldsToJump = FieldsWhereCanJump(f);
        Debug.Log(fieldsToJump.Count);
        var pom = emptyFields.Union(fieldsToJump);
        foreach (var field in pom)
        {
            field.prefab.transform.GetComponent<Renderer>().material.color = c;
        }
    }

    public void ClearHighlihtOnBoard()
    {
        foreach(var field in fields)
        {
            field.prefab.transform.GetComponent<Renderer>().material.color = Color.white;
            field.prefab.transform.position = new Vector3(field.prefab.transform.position.x, field.prefab.transform.position.y,0.5f);
        }
    }

}

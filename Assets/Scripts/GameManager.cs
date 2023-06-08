using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Logic;

[System.Serializable]
public struct Field
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
}

public class GameManager : MonoBehaviour
{
    private float x = -0.02f;
    private float y = 4.25f;
    private readonly int fieldsNumber = 81;
    private readonly int numberOfRows = 17;
    private readonly int[] numberOfColumnsInRow = { 1,2,3,4,5,6,7,8,9,8,7,6,5,4,3,2,1 };
    private Field[] fields;

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
                fields[index] = new Field()
                {
                    id = index++,
                    rowNumber = i,
                    columnNumber = j,
                    prefab = Instantiate(fieldPrefab, new Vector3(x,y,0), Quaternion.identity)
                };
                x+=0.7f;
            }

            x = i >= 8 ? firstFieldX + 0.355f : firstFieldX - 0.355f;
            y -= 0.5f;
        }
    }

    void CreatePlayer1()
    {
        for(int i = 0; i < 10; i++)
        {
            Instantiate(player1PawnPrefab, fields[i].prefab.transform.position, Quaternion.identity);
        }
    }

    void CreatePlayer2()
    {
        for (int i = fieldsNumber - 1; i >= fieldsNumber - 10; i--)
        {
            Instantiate(player2PawnPrefab, fields[i].prefab.transform.position, Quaternion.identity);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Logic;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;

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
    private Color highlightColor = Color.green;
    private Color clearColor = Color.white;
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
    public bool humanPlaying = false;
    public Vector2 goalPosition;
    public Field goalField;
    public Field startField;
    public int movedCheckerId;
    public bool isPawnClicked = false;
    public Game game;
    public int playerMoving = 0;
    public PlayerColor playerColor;
    public int opponentPlayer = 1;
    public PlayerColor oppColor;

    public PlayerType player0Type;
    public PlayerType player1Type;

    public int frameCounter = 0;

    public GameObject player1PawnPrefab;
    public GameObject player2PawnPrefab;
    public GameObject fieldPrefab;

    public bool humanPlayerEndedTurn = false;

    private TMP_Dropdown player0Dropdown;
    private TMP_Dropdown player1Dropdown;
    private TMP_InputField seedInputField;
    private bool gameStarted = false;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {

        seedInputField = GameObject.Find("SeedInputField").GetComponent<TMP_InputField>();
        player0Dropdown = GameObject.Find("Player1Dropdown").GetComponent<TMP_Dropdown>();
        PopulateDropDownWithEnum(player0Dropdown, player0Type);
        player1Dropdown = GameObject.Find("Player2Dropdown").GetComponent<TMP_Dropdown>();
        PopulateDropDownWithEnum(player1Dropdown, player1Type, false);
        CreateBoard();
        CreatePlayer1();
        CreatePlayer2();

        //game = new Game (player0Type, player1Type);
        //player0Type = PlayerType.Human;
        //player1Type = PlayerType.UCT;
        //playerColor = game.players[playerMoving].color;
        //oppColor = game.players[opponentPlayer].color;

        //player0Dropdown = canvas.transform.GetChild(0).GetComponent<Dropdown>();
        //Debug.Log(player0Dropdown);

    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            frameCounter++;

            if ((player0Type != PlayerType.Human && player1Type != PlayerType.Human && frameCounter >= 200) ||
                ((player0Type == PlayerType.Human || player1Type == PlayerType.Human) && humanPlayerEndedTurn))
            {
                frameCounter = 0;
                humanPlayerEndedTurn = false;
                if (game.Win() == PlayerColor.None)
                {
                    var answer = game.players[playerMoving].MakeChoice(game.board, game.players[opponentPlayer]);
                    var oldLogicField = game.board.FindCheckersPosition(answer.checkerIndex, playerColor);
                    Field oldField = fields.First(f => f.mctsX == oldLogicField.x && f.mctsY == oldLogicField.y);
                    Field newField = fields.First(f => f.mctsX == answer.newField.x && f.mctsY == answer.newField.y);

                    game.MoveChecker(answer.newField, answer.checkerIndex, playerMoving);
                    goalField = newField;
                    // znajdujemy checkersa który ma pozycję oldField i robimy dla niego move
                    for (int i = 0; i < playerPawnsNumber; i++)
                    {
                        if (playerMoving == 0)
                        {
                            if (player2Pawns[i].mctsX == oldField.mctsX && player2Pawns[i].mctsY == oldField.mctsY)
                            {
                                player2Pawns[i].mctsX = newField.mctsX;
                                player2Pawns[i].mctsY = newField.mctsY;
                                player2Pawns[i].columnNumber = newField.columnNumber;
                                player2Pawns[i].rowNumber = newField.rowNumber;
                                player2Pawns[i].prefab.transform.position = new Vector3(newField.prefab.transform.position.x, newField.prefab.transform.position.y, -1); // wygenerowane automatycznie, zobaczymy czy zadziała
                                break;
                            }
                        }
                        else
                        {
                            if (player1Pawns[i].mctsX == oldField.mctsX && player1Pawns[i].mctsY == oldField.mctsY)
                            {
                                player1Pawns[i].mctsX = newField.mctsX;
                                player1Pawns[i].mctsY = newField.mctsY;
                                player1Pawns[i].columnNumber = newField.columnNumber;
                                player1Pawns[i].rowNumber = newField.rowNumber;
                                player1Pawns[i].prefab.transform.position = new Vector3(newField.prefab.transform.position.x, newField.prefab.transform.position.y, -1); // wygenerowane automatycznie, zobaczymy czy zadziała
                                break;
                            }
                        }
                    }

                    playerMoving = (playerMoving + 1) % 2;
                    opponentPlayer = (opponentPlayer + 1) % 2;
                    playerColor = game.players[playerMoving].color;
                    oppColor = game.players[opponentPlayer].color;
                }
            }
        }
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
        
        // konwersja współrzędnych planszy na współrzędne z Board
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
            move.playerColor = PlayerColor.Red;
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
            move.playerColor = PlayerColor.Blue;
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

        foreach (var neighbour in adjacentPlayersPawns)
        {
            var adjacentFields = EmptyNeighbourFields(neighbour);
            result.AddRange(adjacentFields.Where(x =>
             {
                 if (x.rowNumber == 7 && f.rowNumber == 9 || x.rowNumber == 9 && f.rowNumber == 7)
                 { 
                     if(Math.Abs(x.columnNumber - f.columnNumber) == 1)
                        return true;
                     return false;
                 }
                 else if (Math.Abs(x.columnNumber - f.columnNumber) == 2 && Math.Abs(x.rowNumber - f.rowNumber) == 2)
                     return true;
                 else if (Math.Abs(x.columnNumber - f.columnNumber) == 2 && Math.Abs(x.rowNumber - f.rowNumber) == 0)
                     return true;
                 else if (Math.Abs(x.columnNumber - f.columnNumber) == 0 && Math.Abs(x.rowNumber - f.rowNumber) == 2)
                     return true;

                 return false;
            }));
        }

        return result;
    }
    
    public bool HighlightPossibleMoveFields(Field f, PlayerColor playerColor)
    {
        var emptyFields = EmptyNeighbourFields(f);
        var fieldsToJump = FieldsWhereCanJump(f);

        var pom = emptyFields.Union(fieldsToJump);

        if(playerColor == PlayerColor.Blue && f.rowNumber <= 3)
        {
            pom = pom.Where(x => x.rowNumber <= 3);
        }

        if (playerColor == PlayerColor.Red && f.rowNumber >= 13)
        {
            pom = pom.Where(x => x.rowNumber >= 13);
        }

        // brak ruchów
        if(!pom.Any())
            return false;

        // podświetl możliwe ruchy
        foreach (var field in pom)
        {
            field.prefab.transform.GetComponent<Renderer>().material.color = highlightColor;
        }

        return true;
    }

    public void ClearHighlihtOnBoard()
    {
        foreach(var field in fields)
        {
            field.prefab.transform.GetComponent<Renderer>().material.color = clearColor;
            field.prefab.transform.position = new Vector3(field.prefab.transform.position.x, field.prefab.transform.position.y,0.5f);
        }
    }

    public void EndHumanPlayerTurn()
    {
        humanPlayerEndedTurn = true;
        playerMoving = (playerMoving + 1) % 2;
        opponentPlayer = (opponentPlayer + 1) % 2;
        playerColor = game.players[playerMoving].color;
        oppColor = game.players[opponentPlayer].color;
    }

    public static void PopulateDropDownWithEnum(TMP_Dropdown dropdown, Enum targetEnum, bool withHuman = true)
    {
        Type enumType = targetEnum.GetType();//Type of enum
        List<TMP_Dropdown.OptionData> newOptions = new();

        for (int i = withHuman ? 0 : 1; i < Enum.GetNames(enumType).Length; i++)//Populate new Options
        {
            newOptions.Add(new TMP_Dropdown.OptionData(Enum.GetName(enumType, i)));
        }

        dropdown.ClearOptions();//Clear old options
        dropdown.AddOptions(newOptions);//Add new options
    }

    public void StartGame()
    {
        player0Type = (PlayerType)player0Dropdown.value;
        player1Type = (PlayerType)(player1Dropdown.value + 1);
        
        int seed = 123;
        if (seedInputField.text != "")
            seed = int.Parse(seedInputField.text);
        game = new Game (player0Type, player1Type,seed);
        
        playerColor = game.players[playerMoving].color;
        oppColor = game.players[opponentPlayer].color;
        gameStarted = true;
    }

}

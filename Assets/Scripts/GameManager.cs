using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] public Board gameBoard;
    [SerializeField] public List<Sprite> piecePictList;
    [SerializeField] public GameObject piecePrefab;
    [SerializeField] public GameObject playerPrefab;
    [SerializeField] public GameObject laserPrefab;
    [SerializeField] public GameObject canvas;
    [SerializeField] public List<GameObject> playerText;
    [SerializeField] public GameObject playerWonText;
    public Dictionary<int, Player> playerDict = new Dictionary<int, Player>();
    private int playerTurn = 1;

    private Dictionary<Tuple<Vector2Int, int>, string> startingPieces = new Dictionary<Tuple<Vector2Int, int>, string>
    {
        { new Tuple<Vector2Int, int>(new Vector2Int(0, 0), 0), "Laser"},
        { new Tuple<Vector2Int, int>(new Vector2Int(0, 2), 0), "Square Knight"},
        { new Tuple<Vector2Int, int>(new Vector2Int(0, 3), 0), "King"},
        { new Tuple<Vector2Int, int>(new Vector2Int(0, 4), 0), "Square Knight"},
        { new Tuple<Vector2Int, int>(new Vector2Int(0, 5), 0), "Triangle Knight"},
        { new Tuple<Vector2Int, int>(new Vector2Int(3, 3), 3), "Triangle Knight"},
        { new Tuple<Vector2Int, int>(new Vector2Int(3, 4), 0), "Triangle Knight"},
        { new Tuple<Vector2Int, int>(new Vector2Int(4, 0), 1), "Triangle Knight"},
        { new Tuple<Vector2Int, int>(new Vector2Int(7, 0), 0), "Splitter"},
        { new Tuple<Vector2Int, int>(new Vector2Int(7, 6), 1), "Triangle Knight"},
    };

    private Dictionary<string, List<bool>> deadSidesDict = new Dictionary<string, List<bool>>
    {
        {"Laser", new List<bool>{false, false, false, false } },
        {"King", new List<bool>{true, true, true, true} },
        {"Splitter", new List<bool>{false, false, false, false } },
        {"Square Knight", new List<bool>{ false, true, true, true } },
        {"Triangle Knight", new List<bool>{false, true, true, false } },
    };

    private Dictionary<string, List<bool>> reflectSidesDict = new Dictionary<string, List<bool>>
    {
        {"Laser", new List<bool>{false, false, false, false } },
        {"King", new List<bool>{false, false, false, false } },
        {"Splitter", new List<bool>{true, true, true, true } },
        {"Square Knight", new List<bool>{ true, false, false, false } },
        {"Triangle Knight", new List<bool>{ true, false, false, true } },
    };

    private Dictionary<String, Sprite> pieceSpriteDictionary = new Dictionary<string, Sprite>();

    private void CreatePieceSpriteDictionary()
    {
        foreach (Sprite sprite in piecePictList)
        {
            pieceSpriteDictionary.Add(sprite.name, sprite);
        }
    }

    private void CreatePieces()
    {
        for (int playerInt = 1; playerInt <= 2; playerInt++)
        {
            foreach (KeyValuePair<Tuple<Vector2Int, int>, string> kvp in startingPieces)
            {
                GameObject newPieceObject = Instantiate(piecePrefab);
                newPieceObject.transform.SetParent(gameBoard.transform);
                Piece newPiece = newPieceObject.GetComponent<Piece>();
                Vector2Int currentPosition = kvp.Key.Item1;
                string pieceName = kvp.Value;
                string fullPieceName;
                int currentRotation = kvp.Key.Item2;
                List<bool> deadSides = deadSidesDict[pieceName];
                List<bool> reflectSides = reflectSidesDict[pieceName];
                if (playerInt == 1)
                {
                    fullPieceName = "White " + pieceName;
                    currentPosition = new Vector2Int(7, 7) - currentPosition;
                }
                else
                {
                    fullPieceName = "Black " + pieceName;
                }

                Sprite pieceSprite = pieceSpriteDictionary[fullPieceName];

                newPiece.CreatePiece(playerInt, currentPosition, pieceName, gameBoard.mAllCells[currentPosition.x, currentPosition.y], pieceSprite, currentRotation, playerDict[playerInt], deadSides, reflectSides);
                newPieceObject.GetComponent<LaserCannon>().laserPrefab = laserPrefab;
                newPieceObject.GetComponent<LaserCannon>().firePosition = newPieceObject.GetComponent<RectTransform>().anchoredPosition;
                newPieceObject.GetComponent<LaserCannon>().canvas = canvas;
                if (pieceName == "Laser")
                {
                    playerDict[playerInt].pLaserCannon = newPieceObject.GetComponent<LaserCannon>();
                }
            }
        }
    }

    private void setPlayers()
    {
        for (int i = 1; i <= 2; i++)
        {
            GameObject player = Instantiate(playerPrefab);
            player.name = "Player " + i.ToString();
            playerDict[i] = player.GetComponent<Player>();
            playerDict[i].SetPlayer(i, this);
            if (i == 1)
            {
                playerDict[i].drehtAn = true;
            }
        }
    }

    public void Start()
    {
        gameBoard.Create(this);
        setPlayers();
        CreatePieceSpriteDictionary();
        CreatePieces();
        HighlightText();
    }

    public void SwitchPlayerTurn()
    {
        playerDict[playerTurn].drehtAn = false;
        playerTurn = (playerTurn % 2) + 1;
        playerDict[playerTurn].drehtAn = true;
        HighlightText();
    }

    public void SendCellSelectedDataToPlayer(Cell cell)
    {
        playerDict[playerTurn].GetTargetCellData(cell);
    }

    public void DeclareWinner(int playerLoser)
    {
        int playerWinner = (playerLoser % 2) + 1;
        playerTurn = 0;
        playerDict[1].drehtAn = false;
        playerDict[2].drehtAn = false;
        playerWonText.GetComponent<Text>().text = "Player " + playerWinner.ToString() + " Won!";
        playerWonText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    public void HighlightText()
    {
        playerText[playerTurn - 1].GetComponent<Outline>().effectDistance = new Vector2(3,3);
        playerText[playerTurn - 1].GetComponent<Text>().color = new Color32(0, 0, 0, 255);
        playerText[playerTurn % 2].GetComponent<Outline>().effectDistance = new Vector2(0,0);
        playerText[playerTurn % 2].GetComponent<Text>().color = new Color32(0, 0, 0, 25);
    }
}

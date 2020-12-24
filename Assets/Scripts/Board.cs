using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] public GameObject mCellPrefab;
    [SerializeField] public GameObject mPiecePrefab;
    public GameManager mGameManager;

    public Cell[,] mAllCells = new Cell[8, 8];

    public void Create(GameManager gameManager)
    {
        mGameManager = gameManager;
        Vector2Int start = new Vector2Int(-420, -420);

        for (int y = 0; y < mAllCells.GetLength(1); y++)
        {
            for (int x = 0; x < mAllCells.GetLength(0); x++)
            {
                GameObject newCell = Instantiate(mCellPrefab, transform);
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 120), (y * 120)) + start;

                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].SetupCell(new Vector2Int(x, y), this);
                if ((x + y) % 2 == 1)
                {
                    newCell.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                    newCell.GetComponent<Cell>().outlineColor = new Color32(255, 255, 255, 255);
                }
                else
                {
                    newCell.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
                    newCell.GetComponent<Cell>().outlineColor = new Color32(0, 0, 0, 255);
                }
            }
        }
    }
}

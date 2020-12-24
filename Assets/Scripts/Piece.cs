using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Piece : EventTrigger
{
    public int mPlayerInt;
    public Vector2Int mCurrentPosition;
    public string mPieceName;
    public Cell mCurrentCell;
    public bool mIsLaser;
    public int mCurrentRotation;
    public Player mPlayer;
    public List<Cell> mHighlightedCells = new List<Cell>();
    public List<bool> mDeadSides;
    public List<bool> mReflectSides;
    public int mReflectType;

    private Dictionary<Vector2, int> mLaserSideDict = new Dictionary<Vector2, int>
    {
        {new Vector2(-1, 0), 0 },
        {new Vector2(0, 1), 1 },
        {new Vector2(1, 0), 2 },
        {new Vector2(0, -1), 3 },
    };

    private Dictionary<int, Vector2Int> mLaserSideDict2 = new Dictionary<int, Vector2Int>
    {
        {0, new Vector2Int(1, 0)},
        {1, new Vector2Int(0, -1)},
        {2, new Vector2Int(-1, 0)},
        {3, new Vector2Int(0, 1)},
    };

    private Dictionary<int, int> mLaserSideDict3 = new Dictionary<int, int>
    {
        {0, 3},
        {1, 2},
        {2, 1},
        {3, 0},
    };



    public void CreatePiece(int playerInt, Vector2Int currentPosition, string pieceName, Cell currentCell, Sprite pieceSprite, int currentRotation, Player player, List<bool> deadSides, List<bool> reflectSides)
    {
        mPlayerInt = playerInt;
        mCurrentPosition = currentPosition;
        mPieceName = pieceName;
        mPlayer = player;
        mCurrentCell = currentCell;
        mIsLaser = pieceName.Contains("Laser");
        mCurrentRotation = currentRotation;
        mDeadSides = deadSides;
        mReflectSides = reflectSides;
        GetComponent<Image>().sprite = pieceSprite;
        PlacePiece(mCurrentCell);
        transform.Rotate(new Vector3Int(0, 0, 1), mCurrentRotation * 90);

        if (pieceName == "Splitter")
        {
            mReflectType = 3;
        }
        else if (pieceName == "Triangle Knight")
        {
            mReflectType = 2;
        }
        else if (pieceName == "Square Knight")
        {
            mReflectType = 1;
        }
        else
        {
            mReflectType = 0;
        }
    }

    public void PlacePiece(Cell newCell)
    {
        mCurrentCell = newCell;
        mCurrentCell.mCurrentPiece = this;
        transform.position = newCell.transform.position;
    }


    private void CheckPathing()
    {
        mHighlightedCells.Add(mCurrentCell);
        if (mIsLaser)
        {
            return;
        }
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                CreateCellPath(x, y);
            }
        }
    }

    private void CreateCellPath(int xDirection, int yDirection)
    {
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;

        currentX += xDirection;
        currentY += yDirection;

        if (currentX < mCurrentCell.mBoard.mAllCells.GetLength(0) && currentX >= 0 && currentY < mCurrentCell.mBoard.mAllCells.GetLength(1) && currentY >= 0)
        {
            if (mCurrentCell.mBoard.mAllCells[currentX, currentY].mCurrentPiece == null)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
            }
        }
    }

    public void MovePiece(Cell cell)
    {
        mCurrentCell.mCurrentPiece = null;
        PlacePiece(cell);
    }

    private void HighlightCells()
    {
        foreach (Cell cell in mHighlightedCells)
        {
            cell.Highlight();
        }
    }

    private void UnhighlightCells()
    {
        foreach (Cell cell in mHighlightedCells)
        {
            cell.Unhighlight();
        }
    }

    public bool Rotate(int rotation)
    {
        if (mIsLaser)
        {
            List<int> BadLazerDirections = new List<int> { 5, 6, -1, -2 };
            if (BadLazerDirections.Contains(rotation + mCurrentRotation + mCurrentPosition.y))
            {
                return false;
            }
        }
        transform.Rotate(new Vector3Int(0, 0, 1), -(mCurrentRotation * 90));
        mCurrentRotation = mCurrentRotation + rotation;
        transform.Rotate(new Vector3Int(0, 0, 1), (mCurrentRotation) * 90);
        return true;
    }

    public void Deselect()
    {
        UnhighlightCells();
        mHighlightedCells = new List<Cell>();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        if (GameObject.FindWithTag("Laser") == null)
        {
            bool isTurn = mPlayer.ReceiveSelectedDataFromPiece(this);
            if (isTurn)
            {
                CheckPathing();
                HighlightCells();
            }
        }
    }

    public void TalkShitGetHit(Laser laser)
    {
        int sideHit = (mLaserSideDict[laser.direction] + mCurrentRotation) % 4;

        int playerRotation = 0;
        if (mPlayerInt == 1)
        {
            playerRotation = 2;
            sideHit = (sideHit + 2) % 4;
        }
        if (mDeadSides[sideHit])
        {
            Destroy(laser.gameObject);
            Destroy(gameObject);
            return;
        }
        else if (mReflectSides[sideHit])
        {
            if (mReflectType == 1)
            {
                GetComponent<LaserCannon>().Fire(laser.direction * -1);
                Destroy(laser.gameObject);
                return;
            }
            else if (mReflectType == 2)
            {
                for (int i = 0; i <= 4; i++)
                {
                    if (i != sideHit && mReflectSides[i])
                    {
                        GetComponent<LaserCannon>().Fire(mLaserSideDict2[(i - mCurrentRotation + 4 + playerRotation) % 4]);
                        Destroy(laser.gameObject);
                        return;
                    }
                }
            }
            else if (mReflectType == 3)
            {
                GetComponent<LaserCannon>().Fire(laser.direction);
                GetComponent<LaserCannon>().Fire(mLaserSideDict2[mLaserSideDict3[(sideHit + mCurrentRotation + playerRotation) % 4]]);
                Destroy(laser.gameObject);
                return;
            }
        }
    }

    private void OnDestroy()
    {
        if (mPieceName == "King")
        {
            mPlayer.Lose();
        }
    }
}

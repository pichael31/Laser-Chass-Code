using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int pPlayerInt;
    public bool drehtAn;
    public GameManager pGameManager;
    public LaserCannon pLaserCannon;
    private bool pMoveMade = false;
    private int pPieceRotated = 0;
    private Cell pCellMovedTo = null;
    private Cell pStartingCell = null;

    private Piece pSelectedPiece;

    public void SetPlayer(int playerInt, GameManager gameManager)
    {
        pPlayerInt = playerInt;
        pGameManager = gameManager;
    }

    public bool ReceiveSelectedDataFromPiece(Piece piece)
    {
        if (drehtAn)
        {
            if (pSelectedPiece != null && piece != pSelectedPiece)
            {
                pSelectedPiece.Deselect();
                if (pMoveMade)
                {
                    if (pPieceRotated != 0)
                    {
                        pSelectedPiece.Rotate(-pPieceRotated);
                        pPieceRotated = 0;
                    }
                    if (pCellMovedTo != null)
                    {
                        pSelectedPiece.PlacePiece(pStartingCell);
                        pCellMovedTo = null;
                    }
                    pMoveMade = false;
                }
            }
            pStartingCell = piece.mCurrentCell;
            pSelectedPiece = piece;
        }
        return drehtAn;
    }

    private void EndTurn()
    {
        pGameManager.SwitchPlayerTurn();
    }

    public void GetTargetCellData(Cell cell)
    {
        if (pSelectedPiece != null && pSelectedPiece.mHighlightedCells.Contains(cell))
        {
            pSelectedPiece.MovePiece(cell);
            pMoveMade = true;
            pCellMovedTo = cell;
        }
    }

    public void Lose()
    {
        pGameManager.DeclareWinner(pPlayerInt);
    }

    private void Update()
    {
        if (drehtAn)
        {
            if (GameObject.FindWithTag("Laser") == null)
            {
                if (pSelectedPiece == null)
                {
                    return;
                }
                else
                {
                    if (pMoveMade)
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            pSelectedPiece.Deselect();
                            pSelectedPiece = null;
                            pPieceRotated = 0;
                            pCellMovedTo = null;

                            pMoveMade = false;

                            pLaserCannon.Fire(pLaserCannon.GetFireDirection());
                            pGameManager.SwitchPlayerTurn();
                            return;
                        }
                        if (Input.GetKeyDown(KeyCode.Q) && pPieceRotated == -1)
                        {
                            bool rotated = pSelectedPiece.Rotate(1);
                            pPieceRotated = 0;
                            pMoveMade = false;
                            return;
                        }
                        if (Input.GetKeyDown(KeyCode.E) && pPieceRotated == 1)
                        {
                            bool rotated = pSelectedPiece.Rotate(-1);
                            pPieceRotated = 0;
                            pMoveMade = false;
                            return;
                        }
                    }
                    else
                    {
                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            bool rotated = pSelectedPiece.Rotate(1);
                            if (rotated)
                            {
                                pPieceRotated = 1;
                                pMoveMade = true;
                                return;
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            bool rotated = pSelectedPiece.Rotate(-1);
                            if (rotated)
                            {
                                pPieceRotated = -1;
                                pMoveMade = true;
                                return;
                            }
                        }
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            pSelectedPiece.Deselect();
                            pSelectedPiece = null;
                            pPieceRotated = 0;
                            pCellMovedTo = null;

                            pMoveMade = false;
                            return;
                        }
                    }
                }
            }
        }
    }
}

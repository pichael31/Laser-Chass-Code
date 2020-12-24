using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Cell : EventTrigger
{
    public Vector2Int mBoardPosition = Vector2Int.zero;
    public Board mBoard;
    public RectTransform mRectTransform;
    public Piece mCurrentPiece;
    public Image outlineImageObject;

    public Color32 outlineColor = new Color32(255, 255, 255, 255);
    private Color32 highlightedOutlineColor = new Color32(217, 224, 29, 255);

    [SerializeField] Sprite outlineImage;
    [SerializeField] Sprite highlightedOutlineImage;

    public void SetupCell(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;
        mRectTransform = GetComponent<RectTransform>();
        
        foreach (Image childImage in GetComponentsInChildren<Image>())
        {
            if (childImage.name == "Outline")
            {
                outlineImageObject = childImage;
            }
        }
        Unhighlight();
    }

    public void Highlight()
    {
        outlineImageObject.sprite = highlightedOutlineImage;
        outlineImageObject.color = highlightedOutlineColor;
    }
    public void Unhighlight()
    {
        outlineImageObject.sprite = outlineImage;
        outlineImageObject.color = outlineColor;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        mBoard.mGameManager.SendCellSelectedDataToPlayer(this);
    }
}

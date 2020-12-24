using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : MonoBehaviour
{
    public Vector3 firePosition;
    public Vector2Int fireDirection;
    public GameObject laserPrefab;
    public GameObject canvas;

    public Vector2Int GetFireDirection()
    {
        int rotation = GetComponentInParent<Piece>().mCurrentRotation;
        int playerInt = GetComponentInParent<Piece>().mPlayerInt;

        if (rotation == 1)
        {
            int x = 0;
            int y = 1;
            return new Vector2Int(x, y) * GetDirFromPlayerInt(playerInt);
        }
        else
        {
            int x = 1;
            int y = 0;
            return new Vector2Int(x, y) * GetDirFromPlayerInt(playerInt);
        }
    }

    public Vector2Int GetDirFromPlayerInt(int playerInt)
    {
        if (playerInt == 1)
        {
            return new Vector2Int(-1, -1);
        }
        else
        {
            return new Vector2Int(1, 1);
        }
    }

    public void Fire(Vector2 direction)
    {
        GameObject laserObject = Instantiate(laserPrefab);
        laserObject.transform.SetParent(canvas.transform);
        Laser laser = laserObject.GetComponent<Laser>();
        laser.Fire(firePosition, direction);
    }
}

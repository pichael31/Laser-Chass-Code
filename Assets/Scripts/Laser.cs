using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private int speed = 250;
    public Vector2 direction;
    public void Fire(Vector3 position, Vector2 directionMoving)
    {
        position.x += directionMoving.x * 25;
        position.y += directionMoving.y * 25;
        GetComponent<RectTransform>().anchoredPosition = position;
        Vector3 newPos = transform.position;
        direction = directionMoving;
        StartCoroutine("MoveLaser");
    }

    IEnumerator MoveLaser()
    {
        while (true)
        {
            GetComponent<RectTransform>().anchoredPosition += direction * speed * Time.deltaTime;
            if (Vector2.Distance(transform.position, new Vector2(960, 540)) > 1000)
            {
                Destroy(gameObject);
            }
                yield return null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.gameObject.GetComponent<Piece>().TalkShitGetHit(this);
    }
}

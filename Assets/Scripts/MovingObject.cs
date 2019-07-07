using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    protected BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    protected Transform CanMove(Vector2 end)
    {
        boxCollider.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(
            transform.position,
            end,
            blockingLayer
        );
        boxCollider.enabled = true;

        if (hit.transform == null)
        {
            return null;
        }

        return hit.transform;
    }

    protected Vector2 GetMoveCoordinates(int xDir, int yDir)
    {
        Vector2 start = transform.position;
        return start + new Vector2(xDir, yDir);
    }

    protected void MoveSmooth(Vector2 end)
    {
        StartCoroutine(SmoothMovement(end));
    }

    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

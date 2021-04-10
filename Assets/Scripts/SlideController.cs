using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    private Vector2 _start;
    [SerializeField] private Vector2 _end;
    public Vector2 end { get { return _end; } set { _end = value; } }
    [SerializeField] private float travelTime = 4f;
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;


    // Start is called before the first frame update
    void Start()
    {
        _start = transform.position;
        // _end = new Vector2(_start.x + _end.x, _start.y + _end.y);

        rb = gameObject.GetComponent<Rigidbody2D>();
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
    }


    void FixedUpdate()
    {

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - .51f), -Vector2.up, .5f);
        if (hit.collider != null && hit.collider.gameObject.tag == "Player")
        {
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
        }
        //the ray collided with something, you can interact
        // with the hit object now by using hit.collider.gameObject
        float t = Mathf.PingPong(Time.time, travelTime) / travelTime;
        rb.MovePosition(Vector2.Lerp(_start, _end, t));

    }
}

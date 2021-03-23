using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    private Vector2 _start;
    [SerializeField] private Vector2 _end;
    [SerializeField] private float travelTime = 4f;
    private Rigidbody2D rb; 

    // Start is called before the first frame update
    void Start()
    {
        _start = transform.position;
        // _end = new Vector2(_start.x + _end.x, _start.y + _end.y);
    
        rb = gameObject.GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {

        float t = Mathf.PingPong(Time.time, travelTime) / travelTime;
        rb.MovePosition(Vector2.Lerp(_start, _end, t));

    }
}

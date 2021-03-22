using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    [SerializeField] private Vector2 _start;
    [SerializeField] private Vector2 _end;
    [SerializeField] private float xDistanceTraveled;
    [SerializeField] private float yDistanceTraveled;
    [SerializeField] private float travelTime = 1f;
    private Rigidbody2D rb; 

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        _start = transform.position; 
        _end = new Vector2(_start.x + xDistanceTraveled, _start.y + yDistanceTraveled);
    }


    void FixedUpdate()
    {

        rb.MovePosition(Vector2.Lerp(_start, _end, travelTime));

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private Vector2 _start;
    [SerializeField] private Vector2 _end;
    Vector2 startPosition;
    [SerializeField] private AudioClip collected;
    private AudioSource audioSource;
    [SerializeField] private float _speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = Camera.main.GetComponent<AudioSource>();
        _start = transform.position; ;
        _end = new Vector2(_start.x, _start.y + .2f);
    }


    void FixedUpdate()
    {

        float t = Mathf.PingPong(Time.time, _speed) / _speed;
        transform.position = Vector2.Lerp(_start, _end, t);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetType() != typeof(CircleCollider2D))
        {

            bool isLastStar = gameObject.name == "LastStar";
            if (isLastStar)
            {
                GameManager.Instance.lastStarPosition = gameObject.transform.position;

            }
            audioSource.PlayOneShot(collected);
            GameManager.Instance.IncrementStarsGathered(isLastStar);
            Destroy(gameObject);
        }
    }
}

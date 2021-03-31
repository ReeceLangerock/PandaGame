using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    bool bounce = false;
    float bounceAmount = 30;
    GameObject Player;
    Rigidbody2D playerRB;
    AudioSource audioSource;
    [SerializeField] public AudioClip jump;

    void Start()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerRB = Player.GetComponent<Rigidbody2D>();
        audioSource = Camera.main.GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (bounce)
        {
            playerRB.velocity = new Vector2(0, 0);
            playerRB.AddForce(transform.up * bounceAmount, ForceMode2D.Impulse);
            audioSource.PlayOneShot(jump);
            bounce = false;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            bounce = true;
        }
    }
}





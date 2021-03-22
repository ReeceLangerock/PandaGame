using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Move player in 2D space
    public CharacterController2D controller2D;
    public float runSpeed = 40f;
    public float jumpHeight = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    bool facingRight = true;
    [SerializeField] float moveDirection = 0;
    public Camera mainCamera;
    Vector3 cameraPos;
    Transform t;
    bool frozen = false;

    // Use this for initialization
    void Start()
    {
        t = transform;
        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls

        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (!frozen)
        {

            // Change facing direction
            if (moveDirection != 0)
            {
                if (moveDirection > 0 && !facingRight)
                {
                    facingRight = true;
                    t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
                }
                if (moveDirection < 0 && facingRight)
                {
                    facingRight = false;
                    t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
                }
            }

            // Jumping
            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                crouch = true;
            }
            else if (Input.GetButtonUp("Crouch"))
            {
                crouch = false;
            }

        }
        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, t.position.y + 2.5f, cameraPos.z);
        }
    }

    void FixedUpdate()
    {
        controller2D.Move(horizontalMove * runSpeed * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "RespawnTrigger")
        {
            Transform respawn = other.transform.Find("Respawn");
            ResetPlayer(respawn);
        }
    }

    public void ResetPlayer(Transform respawn)
    {

        controller2D.transform.position = respawn.position;
    }


}
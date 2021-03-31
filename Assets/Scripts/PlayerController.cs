using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Move player in 2D space
    public CharacterController2D controller2D;
    public Animator animator;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    bool facingRight = true;
    [SerializeField] float moveDirection = 0;
    private Camera mainCamera;
    Vector3 cameraPos;
    Transform t;
    bool frozen = false;
    [SerializeField] private AudioClip falling;
    private AudioSource audioSource;


    // Use this for initialization
    void Start()
    {
        frozen = false;
        t = transform;
        mainCamera = Camera.main;
        if (mainCamera)
        {
            audioSource = mainCamera.GetComponent<AudioSource>();
            cameraPos = mainCamera.transform.position;
        }
    }

    public void Bouncing(){
        
    }

    public void setFrozen(bool isFrozen)
    {
        frozen = isFrozen;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls

        if (!frozen)
            horizontalMove = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
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
            animator.SetBool("isJumping", true);
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

    public void OnLanding()
    {
        // Debug.Log("Onlanding");
        animator.SetBool("isJumping", false);
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
            StartCoroutine(ResetPlayer(respawn));
        }
    }

    public IEnumerator ResetPlayer(Transform respawn)
    {
        audioSource.PlayOneShot(falling);
        frozen = true;
        horizontalMove = 0;
        yield return StartCoroutine(SceneController.Instance.FadeOutAndIn(.25f, 1.75f, .25f));
        controller2D.transform.position = respawn.position;
        yield return new WaitForSeconds(1.75f);
        frozen = false;
    }
}
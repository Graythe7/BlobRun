using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController player;
    public Animator animator;

    private GameManager gameManager;

    private Vector3 direction;

    public float gravity = 9.8f;
    public float jumpForce = 7f;

    private bool isPlayerDead;

    private void Awake()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        animator.updateMode = AnimatorUpdateMode.UnscaledTime; // Use unscaled time for animations
    
    }

    private void Start()
    {
        if (gameManager.isGameStarted)
        {
            // to stop playing player_idle animation and switch to player_Run instead 
            animator.SetBool("isGameStart", true);
        }
    }

    private void OnEnable()
    {
        //reset gravity 
        direction = Vector3.zero; 
    }

    private void Update()
    {
        if (isPlayerDead)
        {
            return; // Stop execution if the player is dead
        }

        //to handle the idle and running animation 
        if (gameManager.isGameStarted)
        {
            animator.SetBool("isGameStart", true);  //idle to run animation
        }
        else
        {
            animator.SetBool("isGameStart", false);  // If not started, keep idle animation
        }


        if (player.isGrounded)
        {
            animator.SetBool("isJumping", false);
            direction = Vector3.down;
            
            if (Input.GetButtonDown("Jump"))
            {
                direction = Vector3.up * jumpForce;
                animator.SetBool("isJumping", true);
                FindObjectOfType<AudioManager>().Play("Player_Jump");
            }
        }
        direction += gravity * Time.deltaTime * Vector3.down;

        player.Move(direction * Time.deltaTime); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            animator.SetBool("Collision", true);
            animator.SetBool("isJumping", false); //to make sure the dead animation plays even if jumping (it worked)
            isPlayerDead = true;

            FindObjectOfType<AudioManager>().Play("Player_Dead");

            gameManager.GameOver();
        }
    }

    public void ResetPlayer()
    {
        isPlayerDead = false;
        animator.SetBool("Collision", false);
        animator.SetBool("Win", false);
        animator.Play("Player_Run");
    }

    public void PlayerWin()
    {
        animator.SetBool("Win", true);
        animator.SetBool("Collision", false);
        animator.SetBool("isJumping", false);

        FindObjectOfType<AudioManager>().Play("Player_Win");

        transform.position = new Vector3(-5f, 1.5f, 0f);

    }
}

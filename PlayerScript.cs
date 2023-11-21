using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Health Things")] 
    private float playerHealth = 1000f;
    private float presentHealth;
    public HealthBar healthBar;



    [Header("Player Movement")]
    public float playerSpeed = 2.5f;
    public float currentPlayerSpeed = 0f;
    public float playerSprint = 5f;
    public float currentPlayerSprint = 0f;


    [Header("Player Camera")]
    public Transform playerCamera;
    public GameObject deathCamera;
    public GameObject EndGameMenuUI;


    [Header("Player Animator and Gravity")]
    public CharacterController cC;
    public float gravity = -9.81f;
    public Animator animator;


    [Header("Player Jumping & velocity")]
    public float jumpRange = 1f;
    public float turnCalmTime = 0.1f;
    float turnCalmVelocity;
    Vector3 velocity;
    public Transform surfaceCheck;
    bool onSurface;
    public float surfaceDistance = 0.4f;
    public LayerMask surfaceMask;


    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        presentHealth = playerHealth;
        healthBar.GiveFullHealth(playerHealth);
    }


    void Update()
    {
        onSurface = Physics.CheckSphere(surfaceCheck.position, surfaceDistance, surfaceMask);

        if(onSurface && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // gravity
        velocity.y += gravity * Time.deltaTime;
        cC.Move(velocity * Time.deltaTime);
        playerMove();
        Jump();
        Sprint();
    }

    void playerMove()
    {
        float horizontal_axis = Input.GetAxisRaw("Horizontal");
        float vertical_axis = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

        if(direction.magnitude >= 0.1f)
        {

            animator.SetBool("Walk", true);
            animator.SetBool("Running", false);
            animator.SetBool("Idle", false);
            animator.SetTrigger("Jump");
            animator.SetBool("AimWalk", false);
            animator.SetBool("IdleAim", false);

            float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,ref turnCalmVelocity, turnCalmTime);
            transform.rotation = Quaternion.Euler(0f,angle,0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            cC.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
            currentPlayerSpeed = playerSpeed;
        }
        else
        {
            animator.SetBool("Idle", true);
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            animator.SetBool("Running", false);
            animator.SetBool("AimWalk", false);
            currentPlayerSpeed = 0f;
        }
    }

    void Jump()
    {
        if(Input.GetButtonDown("Jump") && onSurface)
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Walk", false);
            velocity.y = Mathf.Sqrt(jumpRange * -2 * gravity);
        }
        else
        {
            animator.ResetTrigger("Jump");
        }
    }

    void Sprint()
    {
        if(Input.GetButton("Sprint") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) && onSurface)
        {
            float horizontal_axis = Input.GetAxisRaw("Horizontal");
            float vertical_axis = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal_axis, 0f, vertical_axis).normalized;

            if(direction.magnitude >= 0.1f)
            {

                animator.SetBool("Running", true);
                animator.SetBool("Idle", false);
                animator.SetBool("Walk", false);
                animator.SetBool("IdleAim", false);

                float targetAngle = Mathf.Atan2(direction.x,direction.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle,ref turnCalmVelocity, turnCalmTime);
                transform.rotation = Quaternion.Euler(0f,angle,0f);

                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                cC.Move(moveDirection.normalized * playerSprint * Time.deltaTime);
                currentPlayerSprint = playerSprint;
            }
            else
            {
                animator.SetBool("Idle",false);
                animator.SetBool("Walk",false);
                currentPlayerSpeed = 0f;
            }
        }
    }

    // player hit damage
    public void playerHitDamage(float takeDamage)
    {
        presentHealth -= takeDamage;
        healthBar.SetHealth(presentHealth);
        if(presentHealth <= 0)
        {
            PlayerDie();
        }
    }
    // player death

    private void PlayerDie()
    {
        EndGameMenuUI.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        deathCamera.SetActive(true);
        Object.Destroy(gameObject);
    }
}

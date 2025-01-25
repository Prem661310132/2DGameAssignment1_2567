using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerControl : MonoBehaviour

{
    [Header("PlayerValue")]
    public float speed;
    public float jumpforce;
    public float distance;
    public float checkRadius;
    public Transform groundCheck;

    private float InputHorizontal;
    private float InputVertical;

    AudioManager audioManager;

    private Rigidbody2D rb;
    private bool facingRight = true;

    private bool isGrounded;
    private bool wasGrounded;
    private bool isClimbing;
    private bool Running;

    

    [Header("LayerMask")]
    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    public LayerMask whatIsEnemy;

    [Header("Particle")]
    public GameObject jumpEffect;
    public GameObject LandEffect;

    [Header("Jump")]
    private int extraJumps;
    public int extraJumpsValue;
    public float jumpTimeCounter;
    private float jumptime;

    [Header("Player Animation")]
    private Animator anim;
    public Animator cameraAnimator;
    public Animator PlayerAnimator;

    [Header("Attribute")]
    public Transform attackPoint;
    public float attackRange = 3.5f;
    public int attackDamage = 3;


    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        cameraAnimator = Camera.main.GetComponent<Animator>();
        extraJumps = extraJumpsValue;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        InputHorizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(InputHorizontal * speed, rb.velocity.y);

        PlayerAnimator.SetFloat("velocityX", InputHorizontal * speed);
        PlayerAnimator.SetBool("isRun", Running);

        if (InputHorizontal < 0 || InputHorizontal > 0)
        {
            Running = true;
        }
        else
        {
            Running = false;
        }

        if (facingRight == false && InputHorizontal > 0)
        {
            Flip();
            Debug.Log("Flip Right");
        }
        else if (facingRight == true && InputHorizontal < 0)
        {
            Flip();
          Debug.Log("Flip left");
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
           Attack();
           PlayerAnimator.SetTrigger("Victory");           
           audioManager.PlaySFX(audioManager.playerattack);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            PlayerAnimator.SetTrigger("Hurt");
            audioManager.PlaySFX(audioManager.playerhurt);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayerAnimator.SetBool("isDeath", true);
            audioManager.PlaySFX(audioManager.playerdie);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerAnimator.SetBool("isDeath", false);  
        }


        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.up, distance, whatIsLadder);
        Debug.DrawRay(transform.position, Vector2.up * distance, Color.red);

        if (hitInfo.collider != null)
        {
            Debug.Log("Ladder Detected");
            if (Input.GetKey(KeyCode.UpArrow))
            {
                isClimbing = true;
                Debug.Log("isClimbing = true");
            }
        }
        else
        {
            isClimbing = false;
            Debug.Log("isClimbing = false");
        }

        if (isClimbing == true)
        {
            InputVertical = Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(InputHorizontal * speed, InputVertical * speed);
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1.5f;
        }

    }

    private void Update()
    {
        PlayerAnimator.SetBool("Grounded",isGrounded);

        if (isGrounded && !wasGrounded)
        {
            //anim.SetTrigger("Landing");
            cameraAnimator.SetTrigger("Shake");
            Instantiate(LandEffect, groundCheck.position, Quaternion.identity);
            audioManager.PlaySFX(audioManager.playerlanding);
        }

        wasGrounded = isGrounded;

        if (isGrounded == true)
        {
            extraJumps = extraJumpsValue;
            jumptime = jumpTimeCounter;
           
        }

        if (Input.GetKeyDown(KeyCode.Space) && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpforce;
            extraJumps--;
            Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            audioManager.PlaySFX(audioManager.playerjump);
            //anim.SetTrigger("Jumping");

        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)

        {
            rb.velocity = Vector2.up * jumpforce;
          //anim.SetTrigger("Jumping");

        }


        if (Input.GetKey(KeyCode.Space) && jumptime > 0)
        {
            Debug.Log("jump");
        rb.velocity = Vector2.up * jumpforce;
        jumptime -= Time.deltaTime;
        }

    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

        Debug.Log("Scaler.x: " + transform.localScale.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == ("enemy"))
        {
            Debug.Log("Touch Enemy");
            PlayerAnimator.SetTrigger("Hurt");
            audioManager.PlaySFX(audioManager.playerhurt);
            //PlayerAnimator.SetBool("isDeath", true);
        }

    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatIsEnemy);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemies>().TakeDamage(attackDamage);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(attackPoint.position, attackRange);
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEditor.Timeline;
using UnityEngine;

public class PlayerControl : MonoBehaviour

{
    public float speed;
    public float jumpforce;
    public float distance;
    
    private float InputHorizontal;
    private float InputVertical;

    private Rigidbody2D rb;
    private bool facingRight = true;

    private bool isGrounded;
    private bool wasGrounded;
    private bool isClimbing;
    public Transform groundCheck;
    public float checkRadius;

    public LayerMask whatIsGround;
    public LayerMask whatIsLadder;
    private Animator anim;

    private int extraJumps;
    public int extraJumpsValue;

    public float jumpTimeCounter;
    private float jumptime;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        extraJumps = extraJumpsValue;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        InputHorizontal = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(InputHorizontal * speed, rb.velocity.y);


        if (facingRight == false && InputHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && InputHorizontal < 0)
        {
            Flip();
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
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

    }

    private void Update()
    {
        if (isGrounded && !wasGrounded)
        {
            anim.SetTrigger("Landing");
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
        }
        else if (Input.GetKeyDown(KeyCode.Space) && extraJumps == 0 && isGrounded == true)

        {
            rb.velocity = Vector2.up * jumpforce;
        }


        if (Input.GetKey(KeyCode.Space) && jumptime > 0)
        {
        rb.velocity = Vector2.up * jumpforce;
        jumptime -= Time.deltaTime;
        }

    }
}

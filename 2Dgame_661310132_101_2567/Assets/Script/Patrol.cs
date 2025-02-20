using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float distance;
    public float idleDuration;
    public Animator animator;
    private bool idle = false;
    private bool movingRight = true;
    public Transform groundDetection;

    void Start()
    {
        // ดึง Animator จาก GameObject
        animator = GetComponent<Animator>();
        StartCoroutine(PatrolRoutine());
    }

    void Update()
    {
        if (!idle)
        {
            PatrolMovement();
        }
    }

    void PatrolMovement()
    {
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);

       
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, distance);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    IEnumerator PatrolRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(4f); // เดิน 4 วินาที
            idle = true; // หยุดเดิน
            animator.SetBool("isIdle", true);

            yield return new WaitForSeconds(idleDuration); // รอในสถานะ Idle
            idle = false; // กลับไปเดินต่อ
            animator.SetBool("isIdle", false);

        }
    }
}

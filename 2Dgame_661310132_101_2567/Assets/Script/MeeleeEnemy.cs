using System.Collections;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Settings")]
    public float detectionRadius = 2.5f;
    public float attackCooldown = 2f;
    public int attackDamage = 3;
    private bool isAttacking = false;
    private float cooldownTimer = 3f;

    [Header("References")]
    public Transform attackPoint;
    public Animator animator;
    public LayerMask playerLayer;
    public BoxCollider2D attackHitbox;

    private void Start()
    {
        attackHitbox.enabled = false;
       
    }

    private void Update()
    {
        
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);

        if (player)
        {
            Debug.Log("Player Detected!");
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(attackPoint.position, detectionRadius, playerLayer);
        foreach (Collider2D player in hitPlayer)
        {
            PlayerControl playerControl = player.GetComponent<PlayerControl>();
            if (playerControl != null)
            {
                playerControl.TakeDamage(attackDamage);
                Debug.Log("Player took damage: " + attackDamage);
            }
            else
            {
                Debug.LogWarning("PlayerControl component is missing on the detected player.");
            }
        }
    }



    public void EnableAttackHitbox()
    {
        attackHitbox.enabled = true;
    }

    
    public void DisableAttackHitbox()
    {
        attackHitbox.enabled = false;
        isAttacking = false; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerControl>()?.TakeDamage(attackDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

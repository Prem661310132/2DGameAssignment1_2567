using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public Animator SlimeAnimator;
    public Animator GolemAnimator;
    public Animator SkullAnimator;
    private BoxCollider2D boxCollider;
    [SerializeField] FloatingHeathBar healthbar;
    public enum EnemyType
    {
        Slime,
        Golem,
        Skull
    }

   
    public Dictionary<EnemyType, float> maxHealth = new Dictionary<EnemyType, float>()
    {
        { EnemyType.Slime, 6f },
        { EnemyType.Golem, 15f },
        { EnemyType.Skull, 5f }
    };

    
    private Dictionary<EnemyType, int> currentHealth = new Dictionary<EnemyType, int>();

    public EnemyType enemyType;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        healthbar = GetComponentInChildren<FloatingHeathBar>();

        foreach (var type in maxHealth)
        {
            currentHealth[type.Key] = (int)type.Value;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth.ContainsKey(enemyType))
        {
            currentHealth[enemyType] -= damage;
            Debug.Log("Enemy Type: " + enemyType);
            if (enemyType == EnemyType.Slime)
            {
                SlimeAnimator.SetTrigger("Hurt");
            }
            else if (enemyType == EnemyType.Golem)
            {
                GolemAnimator.SetTrigger("Hit");
                Debug.Log("Golem Hurt.");
            }
            else if (enemyType == EnemyType.Skull)
            {
                SkullAnimator.SetTrigger("Hit");
                Debug.Log("Skull Hurt.");
            }

            healthbar.UpdateHealthBar(currentHealth[enemyType], maxHealth[enemyType]);

            if (currentHealth[enemyType] <= 0)
            {
               
                if (enemyType == EnemyType.Slime)
                {
                    SlimeAnimator.SetTrigger("Die");
                    SlimeAnimator.SetBool("isDead", true);
                }
                else if (enemyType == EnemyType.Golem)
                {
                    GolemAnimator.SetTrigger("Death");
                }
                else if (enemyType == EnemyType.Skull)
                {
                    SkullAnimator.SetTrigger("Death");
                }

                boxCollider.enabled = false;
                StartCoroutine(RemoveAfterDeath());

            }
        }
        else
        {
            Debug.LogError($"EnemyType {enemyType} not found in health dictionary!");
        }
    }
    private IEnumerator RemoveAfterDeath()
    {
        if (enemyType == EnemyType.Slime)
        {
            while (!SlimeAnimator.GetCurrentAnimatorStateInfo(0).IsName("Green Death - Animation"))
            {
                yield return null;
            }
        }
        else if (enemyType == EnemyType.Golem)
        {
            while (!GolemAnimator.GetCurrentAnimatorStateInfo(0).IsName("Golem_Death_B"))
            {
                yield return null;
            }
        }
        else if (enemyType == EnemyType.Skull)
        {
            while (!SkullAnimator.GetCurrentAnimatorStateInfo(0).IsName("Skull_Death"))
            {
                yield return null;
            }
        }

        while (SlimeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null; 
        }

        Destroy(gameObject);
    }


}

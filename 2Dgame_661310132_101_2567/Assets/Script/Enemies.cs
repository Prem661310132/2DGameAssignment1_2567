using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour
{
    public Animator SlimeAnimator;

    public enum EnemyType
    {
        Slime,
        Bat,
        Shroom
    }

   
    public Dictionary<EnemyType, int> maxHealth = new Dictionary<EnemyType, int>()
    {
        { EnemyType.Slime, 10 },
        { EnemyType.Bat, 15 },
        { EnemyType.Shroom, 20 }
    };

    
    private Dictionary<EnemyType, int> currentHealth = new Dictionary<EnemyType, int>();

    public EnemyType enemyType;

    void Start()
    {
        
        foreach (var type in maxHealth)
        {
            currentHealth[type.Key] = type.Value;
        }
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth.ContainsKey(enemyType))
        {
            currentHealth[enemyType] -= damage;
            SlimeAnimator.SetTrigger("Hurt");

            if (currentHealth[enemyType] <= 0)
            {
                SlimeAnimator.SetTrigger("Die");
                
            }
        }
        else
        {
            Debug.LogError($"EnemyType {enemyType} not found in health dictionary!");
        }
    }




}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public EnemyTypes enemyType;
    public EnemyType enemyInstance;

    private void Start()
    {
        enemyType = GetRandomEnemyType();
        enemyInstance = AssignEnemyTypeClass();
        gameObject.name = enemyType.ToSafeString();

        StartCoroutine(enemyInstance.Attack());
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        enemyInstance.Move();
    }


    EnemyType AssignEnemyTypeClass()
    {
        switch (enemyType) // im sure there is a more efficient way of doing this, where i don't need to make a new case for each new enemy, but this will suffice for our purposes.
        {

            /* blank example.
             case EnemyTypes. :
                return new   (transform, GetComponent<Rigidbody2D>());
             */


            case EnemyTypes.TestEnemy1:
                return new TestEnemy1(transform, GetComponent<Rigidbody2D>());
            case EnemyTypes.TestEnemy2:
                return new TestEnemy2(transform, GetComponent<Rigidbody2D>());
            default:
                Debug.LogError("EnemyType Not Found");
                return null;
        }
    }

    public static EnemyTypes GetRandomEnemyType()
    {
        return GameManager.enemyTypesArray[UnityEngine.Random.Range(0, GameManager.enemyTypesArray.Length)];
    }
}


[System.Serializable]
public enum EnemyTypes // used in the selection of enemies
{
    TestEnemy1,
    TestEnemy2,
}



[System.Serializable]
public class EnemyStats
{
    public int currentHP;
    public readonly int maxHp;
    public readonly int attack;
    public readonly int defence;// might not be used;
    public readonly float speed;
    public readonly float attackRate;
    public readonly float attackRange;
    public readonly float attackWindupTime = 0.25f; /// <summary>
                                                    /// amount of time you must stay in range before the attack happens.
                                                    /// </summary>

    public EnemyStats(int maxhp, int attack, int defence, float speed, float attackrate, float attackrange, float attackWindup = 0.25f)
    {
        maxHp = maxhp;
        currentHP = maxHp;
        this.attack = attack;
        this.defence = defence;
        this.speed = speed;
        attackRate = attackrate;
        attackRange = attackrange;
        attackWindupTime = attackWindup;
    }

}

[System.Serializable]
public class EnemyType
{
    public EnemyStats stats;
    public Transform transform;
    public Rigidbody2D rb;


    public virtual IEnumerator Attack()
    {
        while (true)
        {
            if (Vector2.Distance(Player.instance.transform.position, transform.position) < stats.attackRange)
            {
                yield return new WaitForSecondsRealtime(stats.attackWindupTime);
                if (Vector2.Distance(Player.instance.transform.position, transform.position) < stats.attackRange)
                {
                    Player.instance.TakeDamage(stats.attack);
                    float duration = Mathf.Clamp(1 / stats.attackRate - stats.attackWindupTime, 0, Mathf.Infinity); // 
                    yield return new WaitForSecondsRealtime(duration);
                }
            }
            yield return null;
        }
    }

    public virtual void TakeDamage(int damage, bool ignoreDefence = false)
    {
        int damageTaken = ignoreDefence ? damage : damage - stats.defence; // if ignoreDefence, take the full value, otherwise subtract the enemy defence stat from the damage.
        stats.currentHP -= damageTaken;
    }

    public virtual void Move()
    {
        Vector2 movementDirection = (Player.instance.transform.position - transform.position).normalized;
        rb.velocity = movementDirection * stats.speed;
    }
}


public class TestEnemy1 : EnemyType
{
    public TestEnemy1(Transform transform, Rigidbody2D rb)
    {
        this.transform = transform;
        this.rb = rb;
        stats = new(10, 1, 0, 0f, 1f, 2f);

    }

}

public class TestEnemy2 : EnemyType
{
    public TestEnemy2(Transform transform, Rigidbody2D rb)
    {
        this.transform = transform;
        this.rb = rb;
        stats = new(5, 1, 0, 0f, 1f, 2f);

    }

}

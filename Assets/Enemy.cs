using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyTypes enemyType = EnemyTypes.Cockroach;
    public EnemyType enemyInstance;

    private void Start()
    {
        enemyInstance = AssignEnemyTypeClass();
        gameObject.name = enemyType.ToSafeString();


    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartEnemy();
        }
    }

    void StartEnemy() 
    {
        enemyInstance.Move();
        StartCoroutine(enemyInstance.Attack());
    }


    EnemyType AssignEnemyTypeClass()
    {
        switch (enemyType) // im sure there is a more efficient way of doing this, where i don't need to make a new case for each new enemy, but this will suffice for our purposes.
        {

            /* blank example.
             case EnemyTypes. :
                return new   (transform, GetComponent<Rigidbody2D>());
             */


            case EnemyTypes.Cockroach:
                return new Cockroach(transform, GetComponent<Rigidbody>(), GetComponent<NavMeshAgent>());
            case EnemyTypes.Rat:
                return new Rat(transform, GetComponent<Rigidbody>(), GetComponent<NavMeshAgent>());
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
    Cockroach,
    Rat,
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
    public Rigidbody rb;
    public NavMeshAgent navAgent;



    internal EnemyType(Transform transform, Rigidbody rb, NavMeshAgent navAgent)
    {
        this.transform = transform;
        this.rb = rb;
        this.navAgent = navAgent;
    }

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


public class Cockroach : EnemyType
{
    public Cockroach(Transform transform, Rigidbody rb, NavMeshAgent navMeshAgent) : base(transform, rb, navMeshAgent)
    {

        stats = new(10, 1, 0, 1f, 1f, 2f);
        navAgent.speed = stats.speed;

    }

    public override void Move()
    {
        navAgent.SetDestination(GameManager.GetGarbagePilePosition());
        
    }

    public override IEnumerator Attack()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, navAgent.destination) < 0.4f)
            {
                yield return new WaitForSecondsRealtime(stats.attackRate);
                Debug.Log("Hit");
                // Make it effect garbage piles.
            }
            yield return null;
        }
    }

}

public class Rat : EnemyType
{
    Coroutine retargetCoroutine;
    public Rat(Transform transform, Rigidbody rb, NavMeshAgent navMeshAgent) : base(transform, rb, navMeshAgent)
    {

        stats = new(5, 1, 0, 1f, 1f, 2f);
        navAgent.speed = stats.speed;

    }

    public override void Move()
    {
        navAgent.SetDestination(Player.instance.transform.position);
        if (retargetCoroutine != null) { GameManager.instance.StopCoroutine(retargetCoroutine); }
        retargetCoroutine = GameManager.instance.StartCoroutine(RetargetPlayer());

    }

    IEnumerator RetargetPlayer()
    {
        while (true)
        {
            yield return null;
            navAgent.SetDestination(Player.instance.transform.position);
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }



}

public class Raccoon : EnemyType
{
    public Raccoon(Transform transform, Rigidbody rb, NavMeshAgent navMeshAgent) : base (transform, rb, navMeshAgent)
    {

        stats = new EnemyStats(0, 0, 0, 0, 0,0);
        navAgent.speed = stats.speed;
    }
}

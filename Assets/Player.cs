using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerStats playerStats = new (1,1,1,1,1,1,10);
    public static Transform playerTransform;
    [SerializeField] Vector3 movementDirection;
    [SerializeField] Vector3 facingDirection;
    Rigidbody rb;
    [SerializeField] float acceleration;
    [SerializeField] readonly float baseAcceleration =5;
    [SerializeField] float deceleration = 5;
    [SerializeField] float maxSpeed; 
    [SerializeField] readonly float baseMaxSpeed = 5;
    [SerializeField] readonly LayerMask attackLayerMask;
    [SerializeField] bool isCleaningPile;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }

    private void Start()
    {
        GetReferences();
        InitializePlayer();
    }

    void Update()
    {
        GetInput();

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void GetInput()
    {
        var inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"),0, Input.GetAxisRaw("Vertical")).normalized;
        // if the input direction is 0, set movement direction to 0, which allows for decelaration to start.
        // if it is not 0, set the facingdirection AND movementDirection = inputDirection;
        // done this way to maintain facing direction (used for attacking) while allowing for deceleration when movementDirection = (0,0).
        movementDirection = inputDirection != Vector3.zero ? facingDirection = inputDirection : Vector3.zero; 
    }

    void GetReferences()
    {
        rb = GetComponent<Rigidbody>();
        playerTransform = transform;
    }

    void InitializePlayer()
    {
        StartCoroutine(PlayerAttack());
        ApplyStatChanges();
    }


    void MovePlayer()
    {
        if (movementDirection == Vector3.zero) // if no directional input is held, decelerate the player.
        {
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, deceleration);
        }
        else { rb.velocity -= movementDirection * (acceleration); }// otherwise, accelerate the player in the direction held.
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed); // clamp velocity magnitute to cap the players speed.
    }

    public void ApplyStatChanges() 
    {
        maxSpeed = baseMaxSpeed + (playerStats.speed * 0.05f);
        acceleration = baseAcceleration + (playerStats.speed * 0.2f);
    }

    public void TakeDamage(int damage, bool ignoreDefence = false)
    {
        int damageTaken = ignoreDefence ? damage : damage - playerStats.defence; // if ignoreDefence, take the full value, otherwise subtract the enemy defence stat from the damage.
        playerStats.currentHP -= damageTaken;
    }

    public IEnumerator PlayerAttack()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Space) && !isCleaningPile)
            {   
                // calculate size and position of attack box.
                Vector2 attackSize = Vector2.one * playerStats.attackRange;
                Vector2 attackPosition = transform.position + facingDirection;
                // find all enemy colliders.
                Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPosition, attackSize / 2f, 0f, attackLayerMask);
                foreach (Collider2D collider in colliders)
                {
                    collider.GetComponent<Enemy>().enemyInstance.TakeDamage(playerStats.attack);
                }
                DebugDrawOverlapBox(attackPosition, attackSize, Color.red);
                yield return new WaitForSecondsRealtime(1 / playerStats.attackRate);
            }
            yield return null;
        }
    }

    void DebugDrawOverlapBox(Vector2 position, Vector2 size, Color color)
    {
        // get half size of the box to find corner positions.
        Vector2 halfExtents = size / 2f;
    
        Debug.DrawLine(position + new Vector2(-halfExtents.x, -halfExtents.y), position + new Vector2(halfExtents.x, -halfExtents.y), color);
        Debug.DrawLine(position + new Vector2(-halfExtents.x, -halfExtents.y), position + new Vector2(-halfExtents.x, halfExtents.y), color);
        Debug.DrawLine(position + new Vector2(halfExtents.x, -halfExtents.y), position + new Vector2(halfExtents.x, halfExtents.y), color);
        Debug.DrawLine(position + new Vector2(-halfExtents.x, halfExtents.y), position + new Vector2(halfExtents.x, halfExtents.y), color);
    }
}


/// <summary>
/// Stores all stat info about the player. 
/// </summary>
[System.Serializable]
public class PlayerStats
{
    public int currentHP;
    public int maxHP;
    public int attack;
    public int defence;
    public float speed;
    public float attackRate;
    public float attackRange;
    public float cleanStrength;

    public PlayerStats(int maxhp, int attack, int defence, float speed, float attackrate, float attackrange, float cleanStrength)
    {
        maxHP = maxhp;
        currentHP = maxhp;
        this.attack = attack;
        this.defence = defence;
        this.speed = speed;
        this.attackRate = attackrate;
        this.attackRange = attackrange;
        this.cleanStrength = cleanStrength;
    }
}

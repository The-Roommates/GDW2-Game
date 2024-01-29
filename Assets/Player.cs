using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerStats playerStats = new(0, 0, 0, 0, 0, 0);
    [SerializeField] Vector2 movementDirection;
    Rigidbody2D rb;
    [SerializeField] float acceleration, baseAcceleration;
    [SerializeField] float deceleration;
    [SerializeField] float maxSpeed, baseMaxSpeed;
    [SerializeField] LayerMask attackLayerMask;


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

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MovePlayer();
        ApplyStatChanges();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerAttack();
        }
    }

    void GetInput()
    {
        var inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        movementDirection = inputDirection != Vector2.zero ? inputDirection : Vector2.zero; // if the player lets go of all movement, set movement velocity to 0, which in turn starts deceleration.
    }

    void GetReferences()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void InitializePlayer()
    {
        StartCoroutine(PlayerAttack());
    }


    void MovePlayer()
    {
        if (movementDirection == Vector2.zero)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration);
        }
        else { rb.velocity += movementDirection * (acceleration); }



        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    public void ApplyStatChanges()
    {
        maxSpeed = baseMaxSpeed + (playerStats.speed * 0.05f);
        acceleration = baseAcceleration + (playerStats.speed * 0.2f);
    }

    public IEnumerator PlayerAttack()
    {
        float remainingTime = 0;
        while (true)
        {
            if (Input.GetKey(KeyCode.Space) && remainingTime <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapBoxAll((Vector2)transform.position + movementDirection, new Vector2(1, 1) / 2f, 0f, attackLayerMask);
                DebugDrawOverlapBox((Vector2)transform.position + movementDirection, new(1, 1), Color.red);
                remainingTime = 1 / playerStats.attackRate;
                Debug.Log("attack");
            }
            yield return null;
            remainingTime -= Time.deltaTime; 
        }
    }

    void DebugDrawOverlapBox(Vector2 position, Vector2 size, Color color)
    {
        // Calculate the half extents of the box
        Vector2 halfExtents = size / 2f;

        // Draw the box wireframe in the editor
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
    public int curHP;
    public int maxHP;
    public int attack;
    public int defence;
    public float speed;
    public float attackRate;
    public float attackRange;

    public PlayerStats(int maxhp, int attack, int defence, float speed, float attackrate, float attackrange)
    {
        maxHP = maxhp;
        curHP = maxhp;
        this.attack = attack;
        this.defence = defence;
        this.speed = speed;
        this.attackRate = attackrate;
        this.attackRange = attackrange;
    }
}

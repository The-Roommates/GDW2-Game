using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public PlayerStats playerStats;
    [SerializeField] Vector2 movementDirection;
    Rigidbody2D rb;
    public float acceleration;
    public float deceleration;
    public float maxSpeed;


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
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        MovePlayer();
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


    void MovePlayer()
    {
        if (movementDirection == Vector2.zero)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, deceleration);
        }
        else { rb.velocity += movementDirection * acceleration; }



        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    public void PlayerAttack()
    {

    }
}

public class PlayerStats
{
    public int curHp;
    public int maxHp;
    public int attack;
    public int defence;
    public int speed;
    public int attackRate;
    public int attackRange;
}

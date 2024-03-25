using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

public class T_PlayerMovement : MonoBehaviour
{
    public static T_PlayerMovement instance;
    //Player Definition
    Rigidbody rb;
    Rigidbody2D body;
    [Space]
    [SerializeField] bool useZAxis;
    bool canMove = true;
    float speed = 4f;
    float horizontal;
    float vertical;

    //Attacks
    public GameObject _attckSweep;
    public GameObject _attckSpray;
    public bool sweepAttacking = false;
    public bool sprayAttacking = false;

    //Win
    public GameObject win;

    public PlayerStats playerStats = new(10, 1, 0, 0, 0, 0, 0);

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }
    // Start is called before the first frame update
    void Start()
    {
        if(!TryGetComponent(out Rigidbody2D body))
        {
            rb=GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.I))
        {
            SweepAttack();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            SprayAttack();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            win.SetActive(true);
        }

        //Rotating Sweep Attack
        _attckSweep.transform.Rotate(0, 0, 120);

    }

    void FixedUpdate()
    {
        Move();

    }
    void Move()
    {
        if (!canMove) { return; }
        if (body != null)
        {
            body.velocity = new Vector2(horizontal * speed, vertical * speed);
        }
        else
        {
            rb.velocity = new Vector3(horizontal * speed, 0, vertical * speed);
        }
    }
    

    void SweepAttack()
    {
        sweepAttacking = true;
        _attckSweep.SetActive(true);

        StartCoroutine(StopSweepAttack());

    }
    IEnumerator StopSweepAttack()
    {
        yield return new WaitForSeconds(1);

        sweepAttacking = false;
        _attckSweep.SetActive(false);

        yield return new WaitForSeconds(1);
    }

    void SprayAttack()
    {

    }

    public void CheckKillPlayer()
    {
        if (playerStats.currentHP <= 0) { 
        
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            canMove = false;
            GameManager.instance.GameOver();
        }
    }

}

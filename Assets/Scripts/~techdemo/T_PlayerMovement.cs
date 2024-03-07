using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class T_PlayerMovement : MonoBehaviour
{
    //Player Definition
    Rigidbody2D body;

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


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
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
        body.velocity = new Vector2(horizontal * speed, vertical * speed);

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

}

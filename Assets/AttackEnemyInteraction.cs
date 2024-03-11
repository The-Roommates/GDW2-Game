using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemyInteraction : MonoBehaviour
{
    public GameObject broom;

    //Enemies
    public GameObject roach;
    public Rigidbody2D enemyRoach;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Checked");
            enemyRoach.AddForce(enemyRoach.velocity * 100f);
        }

    }*/

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Vector2 dir = (transform.position - roach.transform.position).normalized;

            Debug.Log("Checked");
            //enemyRoach.AddForce(enemyRoach.transform.position * 100f);
            //enemyRoach.AddForce(enemyRoach.transform.position * 100f);
            enemyRoach.AddForce(dir * 300f);
        }
    }
}

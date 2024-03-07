using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDefinition : MonoBehaviour
{
    [SerializeField] private float health;

    Renderer ren;



    public Rigidbody2D rb;

    



    private void Start()
    {
        ren=GetComponent<Renderer>();

    }

    private void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {

        health -= damage;
        //rb.AddForce(transform.localPosition.normalized * -100f);
        rb.AddForce(rb.velocity * 100f);





        if (health > 0f)
        {
            StartCoroutine(_hitFX());

        }

        if (health <= 0f)
        {
            Destroy(gameObject);
            Debug.Log("RIP");


        }
    }

    IEnumerator _hitFX()
    {

        ren.material.color = Color.black;

        yield return new WaitForSeconds(5);
        ren.material.color = Color.black;
        



    }
 


}

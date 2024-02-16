using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoachBehaviour : MonoBehaviour
{
    public GameObject playerCoords;

    public GameObject Roach;
    public Transform parent;
    public Vector3 spawnpoint;
    public Quaternion newRotation;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerCoords.transform.position, 1 * Time.deltaTime);

        //transform.LookAt(playerCoords.transform);

        
       
    }


    /*
    IEnumerator AutoSpawn()
    {
        yield return new WaitForSeconds(10);
        Instantiate(Roach,spawnpoint,newRotation,parent);
        yield return new WaitForSeconds(10);
    }*/

    void getHit()
    {

    }

   


}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static EnemyTypes[] enemyTypesArray = (EnemyTypes[])Enum.GetValues(typeof(EnemyTypes)); // on awake, turns the EnemyTypes enum into an array so that i can randomly select them with an index number.
    public static Transform[] garbagePiles;
    public static float difficultyModifier;
    public GameObject cleanlinessBarPrefab;
    public Transform cleanlinessBarHolder;
    public Gradient cleanlinessBarGradient;
    public LayerMask enemyLayerMask;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }

        Application.targetFrameRate = 60;
        
        // fill garbagepiles with the children of gamemanaager object.
        garbagePiles =  GetComponentsInChildren<Transform>().Where(childTransform => childTransform != transform).ToArray();
    }


    public static Vector3 GetGarbagePilePosition()
    {
        
        return garbagePiles[UnityEngine.Random.Range(0, garbagePiles.Length)].position;
    }
    public static Vector3 GetGarbagePilePosition(int index)
    {
        return garbagePiles[index].position;
    }

}

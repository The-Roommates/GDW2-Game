using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static EnemyTypes[] enemyTypesArray = (EnemyTypes[])Enum.GetValues(typeof(EnemyTypes)); // on awake, turns the EnemyTypes enum into an array so that i can randomly select them with an index number.

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else { Destroy(this); }
    }
}

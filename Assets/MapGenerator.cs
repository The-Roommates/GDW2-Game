using System;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    
    [SerializeField] int quantity = 1000;
    [SerializeField] float minDistance;
    [SerializeField] readonly GameObject[] prefabs;
    List<Vector2> spawnedPositions = new List<Vector2>();
    Vector2 squareSize;
    readonly int maxAttempts = 50;

    void Start()
    {
        squareSize = transform.localScale;
        DisperseObjectsInArea();
    }

    void DisperseObjectsInArea()
    {
        for (int i = 0; i < quantity; i++)
        {
            // try to find a valid position within a number of tries
            int attempts = 0;
            Vector3 randomPosition;

            while (true)
            {
                randomPosition = GetRandomPosition();
                attempts++;

                if (IsValidPosition(randomPosition))
                {
                    // add the valid position to the list
                    spawnedPositions.Add(randomPosition);

                    // instantiate objects at the valid position
                    Instantiate(prefabs[UnityEngine.Random.Range(0, prefabs.Length)], randomPosition, Quaternion.identity);
                    break;
                }
                if (attempts >= maxAttempts)
                {
                    break;
                }
            }


        }
    }


    Vector2 GetRandomPosition()
    {
        float randomX = UnityEngine.Random.Range(-squareSize.x / 2f, squareSize.x / 2f);
        float randomY = UnityEngine.Random.Range(-squareSize.y / 2f, squareSize.y / 2f);

        return new Vector2(randomX, randomY);
    }

    bool IsValidPosition(Vector2 position)
    {
        foreach (Vector2 spawnedPosition in spawnedPositions)
        {
            float distance = Vector2.Distance(position, spawnedPosition);
            if (distance < minDistance)
            {
                return false; // too close to another object
            }
        }
        return true; // valid position
    }
}


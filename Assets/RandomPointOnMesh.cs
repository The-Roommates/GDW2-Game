using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPointOnMesh : MonoBehaviour
{
    private MeshFilter meshFilter;
    public int quantity;

    void Start()
    {
        // Ensure there is a MeshFilter component attached to the GameObject
        meshFilter = GetComponent<MeshFilter>();

        if (meshFilter == null || meshFilter.mesh == null)
        {
            Debug.LogError("MeshFilter or mesh not found.");
            return;
        }
        for (int i = 0; i < quantity; i++)
        {
            // Get random point on the surface of the mesh
            Vector3 randomPoint = GetRandomPointOnMesh();

            Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), randomPoint, Quaternion.Euler(Vector3.zero));
        }
    }

    Vector3 GetRandomPointOnMesh()
    {
        // Access the mesh vertices and triangles
        Vector3[] vertices = meshFilter.mesh.vertices;
        int[] triangles = meshFilter.mesh.triangles;

        // Calculate the total surface area of the mesh
        float totalArea = 0f;
        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            totalArea += Vector3.Cross(v1 - v0, v2 - v0).magnitude * 0.5f;
        }

        // Generate a random area-weighted index
        float randomArea = Random.Range(0f, totalArea);
        float accumulatedArea = 0f;
        int randomIndex = 0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = vertices[triangles[i]];
            Vector3 v1 = vertices[triangles[i + 1]];
            Vector3 v2 = vertices[triangles[i + 2]];

            float triangleArea = Vector3.Cross(v1 - v0, v2 - v0).magnitude * 0.5f;

            accumulatedArea += triangleArea;

            if (accumulatedArea >= randomArea)
            {
                randomIndex = i;
                break;
            }
        }

        // Get the random triangle vertices
        Vector3 randomPoint = GetRandomPointOnTriangle(
            vertices[triangles[randomIndex]],
            vertices[triangles[randomIndex + 1]],
            vertices[triangles[randomIndex + 2]]
        );

        // Transform the local coordinates to world space
        return meshFilter.transform.TransformPoint(randomPoint);
    }

    Vector3 GetRandomPointOnTriangle(Vector3 v0, Vector3 v1, Vector3 v2)
    {
        // Generate random barycentric coordinates
        float r1 = Random.Range(0f, 1f);
        float r2 = Random.Range(0f, 1f - r1);

        // Calculate the random point on the triangle
        return v0 + r1 * (v1 - v0) + r2 * (v2 - v0);
    }
}
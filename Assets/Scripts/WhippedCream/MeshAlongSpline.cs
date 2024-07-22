using UnityEngine;
using System.Collections.Generic;

public class MeshAlongSpline : MonoBehaviour
{
    public BezierSolution.BezierSpline Spline;
    public GameObject meshPrefab;
    public float spacing = 1f;

    public List<Vector3> meshPositions { get; private set; } = new List<Vector3>();
    private List<GameObject> GeneratedMeshes { get; set; } = new List<GameObject>();

    private void Start()
    {
        CleanMeshes();
        CreateMeshesAlongSpline();
    }

    public void CreateMeshesAlongSpline()
    {
        if (meshPrefab == null)
        {
            Debug.LogError("Mesh prefab is not assigned.");
            return;
        }

        meshPositions.Clear(); // Clear previous positions if any

        float splineLength = Spline.Length;
        int numberOfMeshes = Mathf.CeilToInt(splineLength / spacing);

        for (int i = 0; i < numberOfMeshes; i++)
        {
            float t = (float)i / (numberOfMeshes - 1);
            Vector3 position = Spline.GetPoint(t);
            Quaternion rotation = Quaternion.LookRotation(Spline.GetTangent(t));

            GeneratedMeshes.Add(Instantiate(meshPrefab, position, rotation, transform));
            meshPositions.Add(position); // Store mesh position
        }
    }
    private void CleanMeshes()
	{
        foreach(GameObject go in GeneratedMeshes)
		{
            DestroyImmediate(go);
		}
	}
}

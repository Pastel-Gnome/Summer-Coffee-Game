using UnityEngine;
using System.Collections.Generic;

public class PlayerPathComparer : MonoBehaviour
{
    public static PlayerPathComparer comparer;
    public MeshAlongSpline meshAlongSpline; // Reference to the correct spline
    public PlayerPathDrawer playerPathDrawer; // Reference to Player Drawen spline

    public float DifferenceThreshold { get; private set; } = 1f;

    public bool DrawingIsComplete { get; private set; } = false;
	private void Awake()
	{
		if(comparer == null)
		{
            comparer = this;
		}
		else
		{
            DestroyImmediate(comparer);
		}
	}
	private void Update()
    {
        DrawingIsComplete = (playerPathDrawer.pathPoints.Count <= meshAlongSpline.meshPositions.Count) ? false : true;
        if (DrawingIsComplete)
        {
            CompareSplines();
        }
    }

    private void CompareSplines()
    {
        var playerPathPointsList = playerPathDrawer.GetPathPoints();
        var splineMeshPositions = meshAlongSpline.meshPositions;

        if (playerPathPointsList.Count == 0 || splineMeshPositions.Count == 0)
        {
            Debug.Log("Path or mesh positions are empty.");
            return;
        }

        // Limit player path points to the number of meshes
        if (playerPathPointsList.Count > splineMeshPositions.Count)
        {
            playerPathPointsList = playerPathPointsList.GetRange(0, splineMeshPositions.Count);
        }

        float totalPathDeviation = 0f;

        for (int i = 0; i < playerPathPointsList.Count; i++)
        {
            Vector3 currentPlayerPathPoint = playerPathPointsList[i];
            Vector3 currentMeshPosition = splineMeshPositions[i]; // Assume order matches

            float pointDeviation = Vector3.Distance(currentPlayerPathPoint, currentMeshPosition);
            totalPathDeviation += pointDeviation;
        }

        float averagePathDeviation = totalPathDeviation / playerPathPointsList.Count;
        Debug.Log($"The difference between the meshes is: {averagePathDeviation}");

        if (averagePathDeviation < DifferenceThreshold)
        {
            Debug.Log("Path is close enough to the target mesh positions!");
        }
        else
        {
            Debug.Log("Path deviates too much from the target mesh positions.");
        }
    }

}

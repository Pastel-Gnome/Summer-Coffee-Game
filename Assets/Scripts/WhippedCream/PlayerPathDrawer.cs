using System.Collections.Generic;
using UnityEngine;

public class PlayerPathDrawer : MonoBehaviour
{
    public GameObject CreamPrefab;
    public float Spacing = 1f;
    public MeshAlongSpline meshAlongSpline;

    public List<Vector3> pathPoints { get; private set; } = new List<Vector3>();
    private Vector3 lastPoint;

    private void Update()
    {
        if (Input.GetMouseButton(0) && !PlayerPathComparer.comparer.DrawingIsComplete) // Left mouse button
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            if (pathPoints.Count == 0 || Vector3.Distance(mouseWorldPosition, lastPoint) > Spacing)
            {
                pathPoints.Add(mouseWorldPosition);
                lastPoint = mouseWorldPosition;
                Instantiate(CreamPrefab, mouseWorldPosition, Quaternion.identity, transform);
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.point;
        }
        return ray.GetPoint(10f); // Default point if raycast misses
    }

    public List<Vector3> GetPathPoints()
    {
        return pathPoints;
    }
}

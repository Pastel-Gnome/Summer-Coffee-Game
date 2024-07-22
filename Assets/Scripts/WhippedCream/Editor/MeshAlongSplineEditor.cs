using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshAlongSpline))]
public class MeshAlongSplineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MeshAlongSpline meshAlongSpline = (MeshAlongSpline)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Execute"))
        {
            meshAlongSpline.CreateMeshesAlongSpline();
        }
    }

}
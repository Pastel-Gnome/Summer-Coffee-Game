using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LiquidPourEffectController))]
public class LiquidPourEffectControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LiquidPourEffectController controller = (LiquidPourEffectController)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Play"))
        {
            controller.Begin();
        }
    }
}

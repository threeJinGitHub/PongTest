using UnityEditor;
using UnityEngine;

namespace Pong.Utils
{
    [CustomEditor(typeof(MiddleLineCreator))]
    public class MiddleLineCreatorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("CreateLine"))
            {
                var dotsCount = serializedObject.FindProperty("_dotsCount").intValue;
                var floatPrefab = (Transform)serializedObject.FindProperty("_floatPrefab").objectReferenceValue;
                var parent = (Transform)serializedObject.FindProperty("_parent").objectReferenceValue;
                CreateLine(dotsCount, floatPrefab, parent);    
            }
        }

        private static void CreateLine(int dotsCount, Transform squarePrefab, Transform parent)
        {
            for (int i = 0; i < dotsCount; i++)
            {
                var pos = new Vector3(0, (dotsCount - 2 * i - 1) * squarePrefab.localScale.y, 0);
                Instantiate(squarePrefab, pos, Quaternion.identity, parent);
            }
        }
    }
}

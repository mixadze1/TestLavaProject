using UnityEngine;
using UnityEditor;

namespace Assets._Scripts.Game
{
    [CustomEditor(typeof(SavesHandler))]
    public class CustomButton : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            SavesHandler savesHandler = (SavesHandler)target;
            if (GUILayout.Button("DeleteAllSaves"))
            {
                savesHandler.DeleteSaves();
            }
        }
    }
}
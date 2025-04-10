using Building.Tools;
using Config;
using UnityEditor;
using UnityEngine;

namespace MyCustomEditor
{
    /// <summary>
    /// This class is a custom editor window for managing building tools.
    /// </summary>
    [System.Serializable]
    public class BuildingToolManagerWindow : EditorWindow
    {
        private BuildingPositionDataConfig myScriptableObject;

        // Add menu item named "Building Tool Manager" to the Window menu
        [MenuItem("Tools/Building Tool")]
        public static void ShowWindow()
        {
            // Get existing open window or if none, make a new one
            GetWindow<BuildingToolManagerWindow>("Building Tool Manager");
        }

        private void OnGUI()
        {
            GUILayout.Label("Assign Building Position Data Asset", EditorStyles.boldLabel);

            myScriptableObject = (BuildingPositionDataConfig)EditorGUILayout.ObjectField(
                "Building Position Config Asset",
                myScriptableObject,
                typeof(BuildingPositionDataConfig),
                false
            );

            if (myScriptableObject != null)
            {
                if (GUILayout.Button(""))
                {
                    
                }
            }
        }

    }
}

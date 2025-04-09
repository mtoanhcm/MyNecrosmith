using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyCustomEditor
{
    public class SceneManagerWindow : EditorWindow
    {
    private Vector2 scrollPosition = Vector2.zero;
    private List<string> scenePaths = new List<string>();
    private GUIStyle sceneButtonStyle;
    private GUIStyle headerStyle;
    private const string SCENES_FOLDER_PATH = "Assets/Scenes";
    
    [MenuItem("Tools/Scene List")]
    public static void ShowWindow()
    {
        GetWindow<SceneManagerWindow>("Folder Scene Manager");
    }

    private void OnEnable()
    {
        RefreshSceneList();
    }

    private void RefreshSceneList()
    {
        scenePaths.Clear();
        
        // Kiểm tra nếu thư mục Scenes tồn tại
        if (Directory.Exists(SCENES_FOLDER_PATH))
        {
            // Tìm tất cả các file .unity trong thư mục Scenes và các thư mục con
            string[] sceneFiles = Directory.GetFiles(SCENES_FOLDER_PATH, "*.unity", SearchOption.AllDirectories);
            
            // Chuyển đổi đường dẫn tuyệt đối thành đường dẫn relative cho Unity
            foreach (string sceneFile in sceneFiles)
            {
                string relativePath = sceneFile.Replace("\\", "/");
                if (relativePath.StartsWith(Application.dataPath))
                {
                    relativePath = "Assets" + relativePath.Substring(Application.dataPath.Length);
                }
                scenePaths.Add(relativePath);
            }
            
            // Sắp xếp theo tên file
            scenePaths = scenePaths.OrderBy(path => Path.GetFileNameWithoutExtension(path)).ToList();
        }
    }

    private void OnGUI()
    {
        InitializeStyles();
        
        GUILayout.Space(10);
        EditorGUILayout.LabelField("Folder Scene Manager", headerStyle);
        GUILayout.Space(10);
        
        if (GUILayout.Button("Refresh Scene List", GUILayout.Height(30)))
        {
            RefreshSceneList();
        }
        
        GUILayout.Space(5);
        EditorGUILayout.LabelField($"Scenes in {SCENES_FOLDER_PATH}:", EditorStyles.boldLabel);
        GUILayout.Space(5);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        if (scenePaths.Count > 0)
        {
            for (int i = 0; i < scenePaths.Count; i++)
            {
                string scenePath = scenePaths[i];
                
                EditorGUILayout.BeginHorizontal("box");
                
                // Lấy tên scene từ đường dẫn
                string sceneName = Path.GetFileNameWithoutExtension(scenePath);
                
                // Hiển thị index
                EditorGUILayout.LabelField($"[{i}]", GUILayout.Width(30));
                
                // Hiển thị shortPath (đường dẫn ngắn gọn trong thư mục Scenes)
                string shortPath = scenePath.Replace(SCENES_FOLDER_PATH + "/", "");
                
                // Hiển thị tên scene dưới dạng button
                if (GUILayout.Button(shortPath, sceneButtonStyle))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(scenePath);
                    }
                }
                
                // Thêm nút để thêm vào Build Settings nếu chưa có
                bool isInBuildSettings = IsSceneInBuildSettings(scenePath);
                
                // Hiển thị trạng thái build settings
                GUI.enabled = !isInBuildSettings;
                if (GUILayout.Button(isInBuildSettings ? "In Build" : "Add to Build", GUILayout.Width(80)))
                {
                    if (!isInBuildSettings)
                    {
                        AddSceneToBuildSettings(scenePath);
                    }
                }
                GUI.enabled = true;
                
                EditorGUILayout.EndHorizontal();
            }
        }
        else
        {
            EditorGUILayout.HelpBox($"No scenes found in {SCENES_FOLDER_PATH}. Make sure the folder exists.", MessageType.Info);
        }
        
        EditorGUILayout.EndScrollView();
        
        GUILayout.Space(10);
        if (GUILayout.Button("Open Build Settings", GUILayout.Height(30)))
        {
            EditorWindow.GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
        }
    }

    private bool IsSceneInBuildSettings(string scenePath)
    {
        return EditorBuildSettings.scenes.Any(scene => scene.path == scenePath);
    }

    private void AddSceneToBuildSettings(string scenePath)
    {
        List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        buildScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = buildScenes.ToArray();
    }

    private void InitializeStyles()
    {
        if (sceneButtonStyle == null)
        {
            sceneButtonStyle = new GUIStyle(GUI.skin.button);
            sceneButtonStyle.alignment = TextAnchor.MiddleLeft;
            sceneButtonStyle.fontStyle = FontStyle.Normal;
            sceneButtonStyle.padding = new RectOffset(10, 10, 5, 5);
        }
        
        if (headerStyle == null)
        {
            headerStyle = new GUIStyle(EditorStyles.largeLabel);
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.fontStyle = FontStyle.Bold;
            headerStyle.fontSize = 16;
        }
    }
}
}

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MyCustomEditor
{
    public class TerrainCapture : EditorWindow
    {
        private List<Terrain> selectedTerrains = new List<Terrain>();
        private int textureResolution = 2048;
        private string savePath = "Assets/TerrainCaptures/";
        private string fileName = "TerrainCapture";
        private float captureHeight = 2000f;
        private bool showSelectedTerrains = false;
        private Vector2 scrollPosition;
        private bool captureAsOne = true;
        private bool captureFromGameView = true;
        private bool useLighting = true;
        private bool cropToTerrainBounds = true;

        [MenuItem("Tools/Terrain Capture")]
        public static void ShowWindow()
        {
            GetWindow<TerrainCapture>("Terrain Capture");
        }

        private void OnGUI()
        {
            GUILayout.Label("Terrain Capture Settings", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Terrain Selection", EditorStyles.boldLabel);

            if (GUILayout.Button("Find All Terrains in Scene"))
            {
                FindAllTerrains();
            }

            EditorGUILayout.Space();
            captureAsOne = EditorGUILayout.Toggle("Capture All as One Texture", captureAsOne);

            // Show selected terrains in a foldout
            showSelectedTerrains = EditorGUILayout.Foldout(showSelectedTerrains, $"Selected Terrains ({selectedTerrains.Count})");
            if (showSelectedTerrains)
            {
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(150));

                for (int i = 0; i < selectedTerrains.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    selectedTerrains[i] = (Terrain)EditorGUILayout.ObjectField($"Terrain {i + 1}", selectedTerrains[i], typeof(Terrain), true);

                    if (GUILayout.Button("Remove", GUILayout.Width(70)))
                    {
                        selectedTerrains.RemoveAt(i);
                        GUIUtility.ExitGUI(); // Prevent GUI errors when modifying list during layout
                    }
                    EditorGUILayout.EndHorizontal();
                }

                EditorGUILayout.EndScrollView();

                if (GUILayout.Button("Add Terrain"))
                {
                    selectedTerrains.Add(null);
                }
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Capture Settings", EditorStyles.boldLabel);

            textureResolution = EditorGUILayout.IntField("Resolution Per Terrain", textureResolution);
            EditorGUILayout.HelpBox("For multiple terrains, this resolution applies to each terrain section.", MessageType.Info);

            captureHeight = EditorGUILayout.FloatField("Capture Height", captureHeight);
            captureFromGameView = EditorGUILayout.Toggle("Use Game View Settings", captureFromGameView);
            useLighting = EditorGUILayout.Toggle("Include Lighting", useLighting);
            cropToTerrainBounds = EditorGUILayout.Toggle("Crop To Terrain Only", cropToTerrainBounds);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Save Settings", EditorStyles.boldLabel);
            savePath = EditorGUILayout.TextField("Save Directory", savePath);
            fileName = EditorGUILayout.TextField("File Name", fileName);

            EditorGUILayout.Space();

            GUI.enabled = selectedTerrains.Count > 0 && !selectedTerrains.Contains(null);
            if (GUILayout.Button("Capture Terrain" + (selectedTerrains.Count > 1 ? "s" : "")))
            {
                if (captureAsOne && selectedTerrains.Count > 1)
                {
                    CaptureMultipleTerrains();
                }
                else
                {
                    CaptureIndividualTerrains();
                }
            }
            GUI.enabled = true;
        }

        private void FindAllTerrains()
        {
            selectedTerrains.Clear();
            Terrain[] allTerrains = FindObjectsOfType<Terrain>();
            selectedTerrains.AddRange(allTerrains);

            if (selectedTerrains.Count == 0)
            {
                EditorUtility.DisplayDialog("No Terrains Found", "No terrains were found in the current scene.", "OK");
            }
        }

        private void CaptureIndividualTerrains()
        {
            if (selectedTerrains.Count == 0 || selectedTerrains.Contains(null))
            {
                EditorUtility.DisplayDialog("Error", "Please select at least one terrain.", "OK");
                return;
            }

            // Create directory if it doesn't exist
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            int counter = 0;
            foreach (Terrain terrain in selectedTerrains)
            {
                counter++;
                string currentFileName = selectedTerrains.Count > 1 ?
                    $"{fileName}_{counter}" : fileName;

                CaptureTerrainTexture(terrain, currentFileName);
            }

            AssetDatabase.Refresh();
            EditorUtility.DisplayDialog("Capture Complete",
                "All terrain textures have been saved to: " + savePath, "OK");
        }

        private void CaptureMultipleTerrains()
        {
            if (selectedTerrains.Count <= 1)
            {
                EditorUtility.DisplayDialog("Error", "Please select multiple terrains to capture as one.", "OK");
                return;
            }

            // Create directory if it doesn't exist
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            // Calculate combined bounds of all terrains
            Bounds combinedBounds = CalculateCombinedTerrainBounds();

            // Create temporary camera
            GameObject tempCameraObj = new GameObject("__TempTerrainCaptureCamera");
            Camera captureCamera = tempCameraObj.AddComponent<Camera>();

            try
            {
                // Position camera above the center of all terrains
                Vector3 center = combinedBounds.center;
                float maxSize = Mathf.Max(combinedBounds.size.x, combinedBounds.size.z);

                tempCameraObj.transform.position = new Vector3(center.x, combinedBounds.max.y + captureHeight, center.z);
                tempCameraObj.transform.rotation = Quaternion.Euler(90, 0, 0); // Look down

                // Setup orthographic camera
                captureCamera.orthographic = true;
                captureCamera.orthographicSize = maxSize / 2; // Half the size to fit all terrains
                captureCamera.nearClipPlane = 0.1f;
                captureCamera.farClipPlane = captureHeight + combinedBounds.size.y + 100f;

                // Important settings for terrain detail visibility
                if (captureFromGameView)
                {
                    // Copy settings from main camera if available
                    Camera mainCamera = Camera.main;
                    if (mainCamera != null)
                    {
                        captureCamera.clearFlags = mainCamera.clearFlags;
                        captureCamera.backgroundColor = mainCamera.backgroundColor;
                        captureCamera.cullingMask = mainCamera.cullingMask;
                    }
                    else
                    {
                        // Default to everything visible
                        captureCamera.clearFlags = CameraClearFlags.SolidColor;
                        captureCamera.backgroundColor = cropToTerrainBounds ? Color.clear : Color.white;
                        captureCamera.cullingMask = -1; // Everything
                    }
                }
                else
                {
                    // Default settings for good terrain capture
                    captureCamera.clearFlags = CameraClearFlags.SolidColor;
                    captureCamera.backgroundColor = cropToTerrainBounds ? Color.clear : Color.white;
                    captureCamera.cullingMask = -1; // Everything
                }

                // Calculate appropriate texture size based on aspect ratio
                int width, height;
                if (combinedBounds.size.x > combinedBounds.size.z)
                {
                    width = textureResolution;
                    height = Mathf.RoundToInt(textureResolution * (combinedBounds.size.z / combinedBounds.size.x));
                }
                else
                {
                    height = textureResolution;
                    width = Mathf.RoundToInt(textureResolution * (combinedBounds.size.x / combinedBounds.size.z));
                }

                // Create render texture with proper aspect ratio
                RenderTexture renderTexture = new RenderTexture(width, height, 24);
                renderTexture.antiAliasing = 4;
                renderTexture.filterMode = FilterMode.Trilinear;

                // Store original camera rect
                Rect origRect = captureCamera.rect;

                // Adjust camera to use the full render texture
                captureCamera.rect = new Rect(0, 0, 1, 1);
                captureCamera.targetTexture = renderTexture;

                // Store original lighting settings if necessary
                bool originalAutoLight = false;
                if (!useLighting)
                {
                    originalAutoLight = RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                    RenderSettings.ambientLight = Color.white;
                }

                // Render to texture
                captureCamera.Render();

                // Restore lighting settings
                if (!useLighting && originalAutoLight)
                {
                    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                }

                // Create texture2D from render texture
                RenderTexture.active = renderTexture;
                Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
                texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply();
                RenderTexture.active = null;

                // If cropping is enabled, crop to non-transparent pixels
                if (cropToTerrainBounds)
                {
                    texture = CropTransparentPixels(texture);
                }

                // Save texture to PNG
                string fullPath = Path.Combine(savePath, fileName + "_combined.png");
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(fullPath, bytes);

                // Clean up render texture
                captureCamera.targetTexture = null;
                captureCamera.rect = origRect;
                DestroyImmediate(renderTexture);

                // Refresh AssetDatabase and select the asset
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("Capture Complete",
                    "Combined terrain texture saved to: " + fullPath +
                    "\nResolution: " + texture.width + "x" + texture.height, "OK");

                // Select the created asset in the Project window
                if (fullPath.StartsWith("Assets/"))
                {
                    TextureImporter importer = AssetImporter.GetAtPath(fullPath) as TextureImporter;
                    if (importer != null)
                    {
                        importer.isReadable = true;
                        importer.alphaIsTransparency = true;
                        importer.textureCompression = TextureImporterCompression.Uncompressed;
                        importer.SaveAndReimport();
                    }
                    Selection.activeObject = AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath);
                }
            }
            finally
            {
                // Always clean up the temporary camera
                DestroyImmediate(tempCameraObj);
            }
        }

        private void CaptureTerrainTexture(Terrain terrain, string currentFileName)
        {
            // Create temporary camera
            GameObject tempCameraObj = new GameObject("__TempTerrainCaptureCamera");
            Camera captureCamera = tempCameraObj.AddComponent<Camera>();

            try
            {
                // Calculate terrain center and size
                TerrainData terrainData = terrain.terrainData;
                Vector3 terrainPos = terrain.transform.position;
                Vector3 terrainSize = terrainData.size;
                Vector3 terrainCenter = terrainPos + new Vector3(terrainSize.x / 2, 0, terrainSize.z / 2);

                // Position camera above terrain (use terrain's highest point plus capture height)
                float terrainMaxHeight = terrainPos.y + terrainSize.y;
                tempCameraObj.transform.position = new Vector3(terrainCenter.x, terrainMaxHeight + captureHeight, terrainCenter.z);
                tempCameraObj.transform.rotation = Quaternion.Euler(90, 0, 0); // Look down at terrain

                // Setup orthographic camera
                captureCamera.orthographic = true;
                captureCamera.orthographicSize = Mathf.Max(terrainSize.x, terrainSize.z) / 2;
                captureCamera.nearClipPlane = 0.1f;
                captureCamera.farClipPlane = captureHeight + terrainSize.y + 100f;

                // Important settings for terrain detail visibility
                if (captureFromGameView)
                {
                    // Copy settings from main camera if available
                    Camera mainCamera = Camera.main;
                    if (mainCamera != null)
                    {
                        captureCamera.clearFlags = mainCamera.clearFlags;
                        captureCamera.backgroundColor = mainCamera.backgroundColor;
                        captureCamera.cullingMask = mainCamera.cullingMask;
                    }
                    else
                    {
                        captureCamera.clearFlags = CameraClearFlags.SolidColor;
                        captureCamera.backgroundColor = cropToTerrainBounds ? Color.clear : Color.white;
                        captureCamera.cullingMask = -1; // Everything
                    }
                }
                else
                {
                    // Default settings for good terrain capture
                    captureCamera.clearFlags = CameraClearFlags.SolidColor;
                    captureCamera.backgroundColor = cropToTerrainBounds ? Color.clear : Color.white;
                    captureCamera.cullingMask = -1; // Everything
                }

                // Calculate appropriate texture size based on aspect ratio
                int width, height;
                if (terrainSize.x > terrainSize.z)
                {
                    width = textureResolution;
                    height = Mathf.RoundToInt(textureResolution * (terrainSize.z / terrainSize.x));
                }
                else
                {
                    height = textureResolution;
                    width = Mathf.RoundToInt(textureResolution * (terrainSize.x / terrainSize.z));
                }

                // Create render texture
                RenderTexture renderTexture = new RenderTexture(width, height, 24);
                renderTexture.antiAliasing = 4;
                renderTexture.filterMode = FilterMode.Trilinear;

                // Assign render texture to camera
                captureCamera.targetTexture = renderTexture;

                // Store original lighting settings if necessary
                bool originalAutoLight = false;
                if (!useLighting)
                {
                    originalAutoLight = RenderSettings.ambientMode == UnityEngine.Rendering.AmbientMode.Skybox;
                    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
                    RenderSettings.ambientLight = Color.white;
                }

                // Render to texture
                captureCamera.Render();

                // Restore lighting settings
                if (!useLighting && originalAutoLight)
                {
                    RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Skybox;
                }

                // Create texture2D from render texture
                RenderTexture.active = renderTexture;
                Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGBA32, false);
                texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                texture.Apply();
                RenderTexture.active = null;

                // If cropping is enabled, crop to non-transparent pixels
                if (cropToTerrainBounds)
                {
                    texture = CropTransparentPixels(texture);
                }

                // Save texture to PNG
                string fullPath = Path.Combine(savePath, currentFileName + ".png");
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(fullPath, bytes);

                // Clean up render texture
                captureCamera.targetTexture = null;
                DestroyImmediate(renderTexture);

                // Log success
                Debug.Log($"Terrain '{terrain.name}' captured and saved to: {fullPath}");
            }
            finally
            {
                // Always clean up the temporary camera
                DestroyImmediate(tempCameraObj);
            }
        }

        private Texture2D CropTransparentPixels(Texture2D source)
        {
            // If cropToTerrainBounds is disabled, just return the source texture
            if (!cropToTerrainBounds)
                return source;

            int width = source.width;
            int height = source.height;
            Color[] pixels = source.GetPixels();

            // Find bounds of non-transparent pixels
            int minX = width;
            int minY = height;
            int maxX = 0;
            int maxY = 0;
            bool foundPixel = false;

            // Use a very low alpha threshold to detect any terrain pixels
            float alphaThreshold = 0.001f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color pixel = pixels[y * width + x];
                    // Check if pixel has any color data at all (not just alpha)
                    if (pixel.a > alphaThreshold ||
                        pixel.r > 0.01f ||
                        pixel.g > 0.01f ||
                        pixel.b > 0.01f)
                    {
                        foundPixel = true;
                        minX = Mathf.Min(minX, x);
                        minY = Mathf.Min(minY, y);
                        maxX = Mathf.Max(maxX, x);
                        maxY = Mathf.Max(maxY, y);
                    }
                }
            }

            // If no pixels found, return original
            if (!foundPixel || minX > maxX || minY > maxY)
            {
                Debug.LogWarning("No visible pixels found in the capture. Returning original texture.");
                return source;
            }

            // Check if the crop area is too small compared to the original
            float cropRatio = (float)((maxX - minX) * (maxY - minY)) / (width * height);
            if (cropRatio < 0.5f) // If less than 50% of the original
            {
                // Add extra padding to avoid excessive cropping
                int padding = Mathf.Max(20, Mathf.Min(width, height) / 10);
                minX = Mathf.Max(0, minX - padding);
                minY = Mathf.Max(0, minY - padding);
                maxX = Mathf.Min(width - 1, maxX + padding);
                maxY = Mathf.Min(height - 1, maxY + padding);
            }

            // Calculate dimensions of cropped texture
            int cropWidth = maxX - minX + 1;
            int cropHeight = maxY - minY + 1;

            // Create new texture with cropped dimensions
            Texture2D result = new Texture2D(cropWidth, cropHeight, TextureFormat.RGBA32, false);

            // Copy pixels from source to result
            Color[] croppedPixels = new Color[cropWidth * cropHeight];
            for (int y = 0; y < cropHeight; y++)
            {
                for (int x = 0; x < cropWidth; x++)
                {
                    croppedPixels[y * cropWidth + x] = pixels[(y + minY) * width + (x + minX)];
                }
            }

            result.SetPixels(croppedPixels);
            result.Apply();

            return result;
        }

        private Bounds CalculateCombinedTerrainBounds()
        {
            // Start with the bounds of the first terrain
            Bounds combinedBounds = new Bounds();
            bool firstBound = true;

            foreach (Terrain terrain in selectedTerrains)
            {
                if (terrain == null) continue;

                TerrainData terrainData = terrain.terrainData;
                Vector3 terrainPos = terrain.transform.position;
                Vector3 terrainSize = terrainData.size;

                // Create bounds for this terrain
                Bounds terrainBounds = new Bounds(
                    terrainPos + new Vector3(terrainSize.x / 2, terrainSize.y / 2, terrainSize.z / 2),
                    terrainSize
                );

                if (firstBound)
                {
                    combinedBounds = terrainBounds;
                    firstBound = false;
                }
                else
                {
                    combinedBounds.Encapsulate(terrainBounds);
                }
            }

            return combinedBounds;
        }
    }
}
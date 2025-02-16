using System;
using System.Collections.Generic;
using Observer;
using UnityEngine;

namespace Fog
{
    public class FogOfWarManager : MonoBehaviour
    {
        private static readonly int ExploredTex = Shader.PropertyToID("_ExploredTex");
        
        [Header("Settings")] public int textureSize = 512; // Resolution of the fog texture
        public float fogRadius = 5f; // Radius of vision for each unit
        public LayerMask fogLayer; // Layer for fog collision detection

        // Textures for current vision and explored areas
        private Texture2D currentVisionTexture;
        private Texture2D exploredTexture;

        // Color arrays for efficient texture updates
        private Color32[] clearColors; // Base fog color array
        private Color32[] currentColors; // Current vision color array
        private Color32[] exploredColors; // Explored areas color array
        private Material fogMaterial; // Reference to the fog material

        // List to track all units that should reveal fog
        private List<Transform> visibleUnits;

        private void Awake()
        {
            visibleUnits = new List<Transform>();
        }

        private void Start()
        {
            // Initialize textures
            currentVisionTexture = new Texture2D(textureSize, textureSize, TextureFormat.R8, false);
            exploredTexture = new Texture2D(textureSize, textureSize, TextureFormat.R8, false);

            currentVisionTexture.wrapMode = TextureWrapMode.Clamp;
            exploredTexture.wrapMode = TextureWrapMode.Clamp;

            // Initialize color arrays
            clearColors = new Color32[textureSize * textureSize];
            currentColors = new Color32[textureSize * textureSize];
            exploredColors = new Color32[textureSize * textureSize];

            // Set initial colors to full fog
            Color32 foggedColor = new Color32(0, 0, 0, 255);
            for (int i = 0; i < clearColors.Length; i++)
            {
                clearColors[i] = foggedColor;
                currentColors[i] = foggedColor;
                exploredColors[i] = foggedColor;
            }

            // Setup material and assign textures
            fogMaterial = GetComponent<MeshRenderer>().material;
            fogMaterial.mainTexture = currentVisionTexture;
            fogMaterial.SetTexture(ExploredTex, exploredTexture);

            UpdateFogTexture();
            
            EventManager.Instance.StartListening<EventData.OnUnitRegisterOpenFogOfWar>(OnRegisterUnitOpenFog);
            EventManager.Instance.StartListening<EventData.OnUnitUnRegisterOpenFogOfWar>(OnUnRegisterUnitOpenFogOfWar);
        }

        private void OnRegisterUnitOpenFog(EventData.OnUnitRegisterOpenFogOfWar data)
        {
            if (!visibleUnits.Contains(data.Unit))
            {
                visibleUnits.Add(data.Unit);
            }
        }

        private void OnUnRegisterUnitOpenFogOfWar(EventData.OnUnitUnRegisterOpenFogOfWar data)
        {
            if (visibleUnits.Contains(data.Unit))
            {
                visibleUnits.Remove(data.Unit);
            }
        }

        private void Update()
        {
            UpdateFogTexture();
        }
        
        private void UpdateFogTexture()
        {
            // Reset current vision
            Array.Copy(clearColors, currentColors, currentColors.Length);
            // Update fog for each visible unit
            foreach (Transform unit in visibleUnits)
            {
                if (unit == null) continue;

                Vector3 unitWorldPos = unit.position;
                Vector2 unitTexturePos = WorldToTexturePosition(unitWorldPos);
                
                // Calculate visible area around unit
                int radius = Mathf.RoundToInt(fogRadius * textureSize);
                int px = Mathf.RoundToInt(unitTexturePos.x);
                int py = Mathf.RoundToInt(unitTexturePos.y);

                // Update fog visibility in unit's radius
                for (int y = -radius; y <= radius; y++)
                {
                    for (int x = -radius; x <= radius; x++)
                    {
                        if (x * x + y * y <= radius * radius)
                        {
                            int texX = px + x;
                            int texY = py + y;

                            if (texX >= 0 && texX < textureSize && texY >= 0 && texY < textureSize)
                            {
                                float distance = Mathf.Sqrt(x * x + y * y) / radius;
                                byte value = (byte)Mathf.Lerp(255, 0, distance);
                                int index = texY * textureSize + texX;

                                // Update both current vision and explored area
                                currentColors[index].r = (byte)Mathf.Max(currentColors[index].r, value);
                                exploredColors[index].r = (byte)Mathf.Max(exploredColors[index].r, value);
                            }
                        }
                    }
                }
            }

            // Apply texture updates
            currentVisionTexture.SetPixels32(currentColors);
            currentVisionTexture.Apply();

            exploredTexture.SetPixels32(exploredColors);
            exploredTexture.Apply();
        }

        // Convert world position to texture coordinates
        private Vector2 WorldToTexturePosition(Vector3 worldPos)
        {
            Vector3 localPos = transform.InverseTransformPoint(worldPos);
            return new Vector2(
                (localPos.x + 0.5f) * textureSize,
                (localPos.z + 0.5f) * textureSize
            );
        }

        // Save the explored state to a file
        public void SaveExploredState()
        {
            byte[] bytes = exploredTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/exploredMap.png", bytes);
        }

        // Load the explored state from a file
        public void LoadExploredState()
        {
            string filePath = Application.persistentDataPath + "/exploredMap.png";
            if (System.IO.File.Exists(filePath))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                exploredTexture.LoadImage(bytes);
                exploredTexture.Apply();

                exploredColors = exploredTexture.GetPixels32();
            }
        }
    }
}

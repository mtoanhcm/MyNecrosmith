using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapGeneratorEditor : OdinEditorWindow
{
    [MenuItem("Tools/Odin/Tilemap Generator")]
    public static void ShowWindow() => GetWindow<TilemapGeneratorEditor>().Show();

    [Title("Tilemap Settings")]
    [Required, PropertyOrder(0)]
    public Tilemap TargetTilemap;

    [ShowIf("TargetTilemap"), PropertyOrder(1), MinValue(2)]
    [SerializeField] private int width = 64;
    public int Width { get => width; set => width = value; }

    [ShowIf("TargetTilemap"), PropertyOrder(2), MinValue(2)]
    [SerializeField] private int height = 64;
    public int Height { get => height; set => height = value; }

    [ShowIf("TargetTilemap"), PropertyOrder(3)]
    [SerializeField] private bool centered = true;
    public bool Centered { get => centered; set => centered = value; }

    [Title("Environment Tile Folder")]
    [ShowIf("TargetTilemap"), PropertyOrder(4)]
    [SerializeField, FolderPath(ParentFolder = "Assets/Tiles", RequireExistingPath = true)]
    private string folder;
    public string Folder { get => folder; set => folder = value; }

    [ShowIf("TargetTilemap"), PropertyOrder(5)]
    [Button("Load Tiles From Folder", ButtonSizes.Medium)]
    private void LoadTilesButton()
    {
        if (string.IsNullOrEmpty(Folder))
        {
            EditorUtility.DisplayDialog("Missing Folder", "Please specify the environment folder name.", "OK");
            return;
        }

        LoadTilesFromFolder();
    }

    [Title("Random Terrain Settings")]
    [ShowIf("TargetTilemap"), PropertyOrder(6)]
    [SerializeField] private int seed = 12345;
    public int Seed { get => seed; set => seed = value; }

    [ShowIf("TargetTilemap"), PropertyOrder(7), Range(0.01f, 1f)]
    [SerializeField] private float noiseScale = 0.05f;
    public float NoiseScale { get => noiseScale; set => noiseScale = value; }

    [ShowIf("TargetTilemap"), PropertyOrder(8), MinValue(1)]
    [SerializeField] private int patchSize = 4;
    public int PatchSize { get => patchSize; set => patchSize = value; }

    [InfoBox("Biome tiles will be loaded from folder inside Assets/Tiles", InfoMessageType.Info)]
    [ShowIf("TargetTilemap"), PropertyOrder(9)]
    [TableList(ShowIndexLabels = true)]
    public List<BiomeConfig> BiomeConfigs = new List<BiomeConfig>();

    [ShowIf("TargetTilemap"), PropertyOrder(10)]
    [Button("Generate Random Map", ButtonSizes.Large), GUIColor(0.2f, 0.8f, 0.2f), EnableIf("CanGenerate")]
    private void GenerateRandomMap() => Generate();

    private BiomeConfig[,] terrainMap;

    private void Generate()
    {
        if (!TargetTilemap)
        {
            EditorUtility.DisplayDialog("Missing Tilemap", "Please assign a TargetTilemap.", "OK");
            return;
        }

        if (BiomeConfigs.Count == 0)
        {
            EditorUtility.DisplayDialog("No Tiles", "Please load environment tiles before generating.", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("Confirm", $"This will generate a {Width}x{Height} map and clear the tilemap.", "Proceed", "Cancel"))
            return;

        Undo.RegisterCompleteObjectUndo(TargetTilemap, "Tilemap Generation");
        TargetTilemap.ClearAllTiles();

        GenerateTerrainMap();
        ApplyTerrainToTilemap();

        EditorUtility.DisplayDialog("Done", "Tilemap generated successfully.\nMap Seed copied to console.", "OK");
    }

    private bool CanGenerate => BiomeConfigs != null && BiomeConfigs.Count > 0;

    public void LoadTilesFromFolder()
    {
        BiomeConfigs.Clear();
        string fullPath = $"Assets/Tiles/{Folder}".Replace("\\", "/");

        if (!Directory.Exists(fullPath))
        {
            EditorUtility.DisplayDialog("Invalid Folder", $"Folder not found: {fullPath}", "OK");
            return;
        }

        string[] guids = AssetDatabase.FindAssets("t:TileBase", new[] { fullPath });

        foreach (var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);
            if (tile == null) continue;

            string name = Path.GetFileNameWithoutExtension(path);
            BiomeConfigs.Add(new BiomeConfig
            {
                Name = name,
                Tile = tile,
                Weight = 33f
            });
        }
    }

    private void GenerateTerrainMap()
    {
        terrainMap = new BiomeConfig[Width, Height];
        Random.InitState(Seed);

        float totalWeight = BiomeConfigs.Sum(b => b.Weight);

        for (int px = 0; px < Width; px += PatchSize)
        {
            for (int py = 0; py < Height; py += PatchSize)
            {
                float nx = (px + Seed) * NoiseScale;
                float ny = (py + Seed) * NoiseScale;
                float noise = Mathf.PerlinNoise(nx, ny);

                BiomeConfig selected = MapNoiseToBiome(noise, totalWeight);

                for (int dx = 0; dx < PatchSize; dx++)
                {
                    for (int dy = 0; dy < PatchSize; dy++)
                    {
                        int x = px + dx;
                        int y = py + dy;
                        if (x < Width && y < Height)
                            terrainMap[x, y] = selected;
                    }
                }
            }
        }
    }

    private BiomeConfig MapNoiseToBiome(float noise, float totalWeight)
    {
        float threshold = 0f;
        foreach (var biome in BiomeConfigs)
        {
            threshold += biome.Weight / totalWeight;
            if (noise <= threshold)
                return biome;
        }
        return BiomeConfigs[0];
    }

    private void ApplyTerrainToTilemap()
    {
        int cx = Centered ? -(Width / 2) : 0;
        int cy = Centered ? -(Height / 2) : 0;

        int total = Width * Height;
        int count = 0;

        try
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("Applying Terrain", $"({x},{y})", (float)count / total))
                    {
                        EditorUtility.ClearProgressBar();
                        EditorUtility.DisplayDialog("Cancelled", "Tilemap generation was cancelled.", "OK");
                        return;
                    }

                    var biome = terrainMap[x, y];
                    var tile = biome?.Tile;
                    TargetTilemap.SetTile(new Vector3Int(x + cx, y + cy, 0), tile);
                    count++;
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
}

[System.Serializable]
public class BiomeConfig
{
    [HorizontalGroup("Biome"), LabelWidth(60), Required]
    public string Name;

    [HorizontalGroup("Biome"), LabelWidth(60), Range(0, 100)]
    public float Weight = 33f;

    [HorizontalGroup("Biome"), LabelWidth(60), Required]
    public TileBase Tile;
}

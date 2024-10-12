using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int[,] levelMap = new int[,]
{ 
{1,2,2,2,2,2,2,2,2,2,2,2,2,7},
{2,5,5,5,5,5,5,5,5,5,5,5,5,4},
{2,5,3,4,4,3,5,3,4,4,4,3,5,4},
{2,6,4,0,0,4,5,4,0,0,0,4,5,4},
{2,5,3,4,4,3,5,3,4,4,4,3,5,3},
{2,5,5,5,5,5,5,5,5,5,5,5,5,5},
{2,5,3,4,4,3,5,3,3,5,3,4,4,4},
{2,5,3,4,4,3,5,4,4,5,3,4,4,3},
{2,5,5,5,5,5,5,4,4,5,5,5,5,4},
{1,2,2,2,2,1,5,4,3,4,4,3,0,4},
{0,0,0,0,0,2,5,4,3,4,4,3,0,3},
{0,0,0,0,0,2,5,4,4,0,0,0,0,0},
{0,0,0,0,0,2,5,4,4,0,3,4,4,0},
{2,2,2,2,2,1,5,3,3,0,4,0,0,0},
{0,0,0,0,0,0,5,0,0,0,4,0,0,0},
};
    public enum TileType { None ,OutWall, InnerWall, StandardSpace, OutCorner, InnerCorner, SpecialSpace, TWall}
   // public TileType[,] levelMap;
    public GameObject[] tilePrefab;

    private int[,] testArr;
    
    void Start()
    {
       // levelMap = new TileType[width, height];
        Destroy(GameObject.Find("Level1"));
        GenerateLevel();
        
        
    }

    [Button]
    void GenerateLevel()
    {
        testArr = new int[levelMap.GetLength(0), levelMap.GetLength(1)];
        
        for (int x = 0; x < levelMap.GetLength(0); x++)
        {
            for (int y = 0; y < levelMap.GetLength(1); y++)
            {
                testArr[x,y] = levelMap[x,y];
            }
        }

        for (var i = 0; i < testArr.GetLength(0); i++)
        {
            Debug.Log("----------");
            string arr = "";
            for (var j = 0; j < testArr.GetLength(1); j++)
            {
                arr += testArr[i, j].ToString() + ",";
            }
            Debug.Log(arr);
        }
    }

    GameObject GetPrefabFromType(int type)
    {
        return tilePrefab[type];
       
    }



    void Update()
    {
        
    }
}

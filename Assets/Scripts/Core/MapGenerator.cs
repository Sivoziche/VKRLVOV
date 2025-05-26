using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MapGenerator : MonoBehaviour
{
    [Header("Component Essentials")]
    public GameObject shipPrefab;
    public GameObject startPrefab; // 1 
    public GameObject finishPrefab; // 2
    public GameObject[] obstaclePrefabs; // 3+
    private Vector2Int mapSize = new(10, 20); // 10 x 20
    private int[,] map;

    void Awake()
    {
        string levelName = getLevelName();
        LoadMap(levelName);
        GenerateMap();
        GameObject go = GameObject.FindWithTag("StartPoint");
        Instantiate(shipPrefab, go.transform.position, Quaternion.identity, transform);
    }

    void LoadMap(string map_name)
    {
        TextAsset mapText = Resources.Load<TextAsset>("Maps/" + map_name);
        if(mapText == null)
        {
            Debug.LogError("Map not found " + map_name);
            return;
        }
        else
        {
            string[] lines = mapText.text.Split('\n');

            if (lines.Length != mapSize.y)
            {
                Debug.LogError($"Ожидалось {mapSize.y} строк, но получено {lines.Length}");
                return;
            }

            map = new int[mapSize.y, mapSize.x];

            for (int i = 0; i < mapSize.y; i++)
            {
                string[] tokens = lines[i].Trim().Split(' ');
                if (tokens.Length != mapSize.x)
                {
                    Debug.LogError($"Строка {i} содержит {tokens.Length} элементов вместо {mapSize.x}");
                    return;
                }

                for (int j = 0; j < mapSize.x; j++)
                {
                    int value;
                    int.TryParse(tokens[j], out value);
                    map[i, j] = value;
                }
            }    
        }
    }

    void GenerateMap()
    {
        for (int i = 0; i < mapSize.y; i++)
        {
            for (int j = 0; j < mapSize.x; j++)
            {
                float x = j - mapSize.x / 2f + 0.5f;
                float y = mapSize.y / 2f - (i + 0.5f);
                Vector2 pos = new Vector2(x, y); // each cell 1x1 unit
                int cellType = map[i, j];

                GameObject prefab = GetPrefabForCell(cellType);
                if (prefab != null)
                {
                    Instantiate(prefab, pos, Quaternion.identity, transform);
                }
            }
        }
    }

    GameObject GetPrefabForCell(int type)
    {
        switch (type)
        {
            case 0: return null;
            case 1: return startPrefab;
            case 2: return finishPrefab;
            default:
                int i = type - 3;
                if (i >= 0 && i < obstaclePrefabs.Length)
                    return obstaclePrefabs[i];
                return null;
        }
    }

    private string getLevelName()
    {
        Scene scene = SceneManager.GetActiveScene();
        string levelName = scene.name;
        Debug.Log("Current level: " + levelName);
        return levelName;
    }
}

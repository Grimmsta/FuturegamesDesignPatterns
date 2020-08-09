using System.Collections.Generic;
using UnityEngine;

public class MapCreator
{
    [SerializeField] private float size = 2;

    private MapReader mapReader;

    private const char charPath = '0';
    private const char charObstacle = '1';
    private const char charTowerOne = '2';
    private const char charTowerTwo = '3';
    private const char charStartEnemyBase = '8';
    private const char charEndPlayerBase = '9';
    private const char mapTileEnd = '#';

    private char currentTileChar;

    private readonly Dictionary<TileType, GameObject> m_PrefabsById;

    private List<string> lines;
    private List<string> enemies;

    private List<Vector2Int> mapTiles;

    private Vector2Int startPos;
    private Vector2Int endPos;

    public List<Vector2Int> MapTiles => mapTiles;

    public Vector2Int StartPos => startPos;
    public Vector2Int Endpos => endPos;
    public List<string> Enemies => enemies;

    public MapCreator(IEnumerable<MapKeyData> mapKeyData, TextAsset txtFile)
    {
        m_PrefabsById = new Dictionary<TileType, GameObject>();
        foreach (MapKeyData data in mapKeyData)
        {
            m_PrefabsById.Add(data.TileType, data.Prefab);
        }

        mapReader = new MapReader();

        lines = new List<string>();
        lines = mapReader.ReadFile(txtFile);

        enemies = new List<string>();
        enemies = mapReader.ReadEnemyWaves(txtFile);

        mapTiles = new List<Vector2Int>();
    }

    public void CreateMap()
    {
        for (int row = lines.Count - 1, rowIndex = 0; row >= 0; row--, rowIndex++)
        {
            string line = lines[row];

            for (int column = 0; column < line.Length; column++)
            {
                char item = line[column];
                float z = rowIndex * size;
                float x = column * size;

                switch (item)
                {
                    case charPath:
                        currentTileChar = charPath;
                        break;
                    case charObstacle:
                        currentTileChar = charObstacle;
                        break;
                    case charTowerOne:
                        currentTileChar = charTowerOne;
                        break;
                    case charTowerTwo:
                        currentTileChar = charTowerTwo;
                        break;
                    case charStartEnemyBase:
                        currentTileChar = charStartEnemyBase;
                        break;
                    case charEndPlayerBase:
                        currentTileChar = charEndPlayerBase;
                        break;
                    case mapTileEnd:
                        break;
                    default:
                        break;
                }

                TileType type = TileMethods.TypeByIdChar[currentTileChar];
                GameObject currentPrefab = m_PrefabsById[type];
                GameObject inst = GameObject.Instantiate(currentPrefab, new Vector3(x, 0, z), Quaternion.identity);

                if (TileMethods.IsWalkable(type))
                {
                    Vector2Int pos = new Vector2Int(Mathf.RoundToInt(inst.transform.position.x),
                        Mathf.RoundToInt(inst.transform.position.z));

                    if (type == TileType.Start)
                    {
                        startPos = pos;
                    }
                    else if (type == TileType.End)
                    {
                        endPos = pos;
                    }

                    mapTiles.Add(pos);
                }
            }
        }
    }
}

public class MapKeyData
{
    public TileType TileType { get; private set; }
    public GameObject Prefab { get; private set; }

    public MapKeyData(TileType tileType, GameObject prefab)
    {
        TileType = tileType;
        Prefab = prefab;
    }
}
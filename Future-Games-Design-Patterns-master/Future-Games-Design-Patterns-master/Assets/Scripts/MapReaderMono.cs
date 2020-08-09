using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapKeyDataMono
{
    [SerializeField] private TileType m_Type;
    [SerializeField] private GameObject m_Prefab;
    public TileType TileType => m_Type;
    public GameObject Prefab => m_Prefab;
}

public class MapReaderMono : MonoBehaviour
{
    [SerializeField] private MapKeyDataMono[] m_MapReaderMono;
    [SerializeField] public TextAsset file;

    private Vector2Int startPos;
    private Vector2Int endPos;
    
    private MapCreator createMap;
    
    private List<MapKeyData> data; 
    private List<Vector2Int> wayPoints;

    public List<Vector2Int> WayPoints => wayPoints;
    public Vector2Int StartPos => startPos;
    public Vector2Int EndPos => endPos;
    public MapCreator MapCreate => createMap;

    private void Awake()
    {
        data = new List<MapKeyData>();
        
        wayPoints = new List<Vector2Int>();

        foreach (MapKeyDataMono readerMono in m_MapReaderMono)
        {
            MapKeyData d = new MapKeyData(readerMono.TileType, readerMono.Prefab);
            data.Add(d);
        }

        createMap = new MapCreator(data, file);
        createMap.CreateMap();

        foreach (Vector2Int item in createMap.MapTiles)
        {
            wayPoints.Add(new Vector2Int(item.x/2, item.y/2));
        }
        
        startPos = new Vector2Int(createMap.StartPos.x / 2, createMap.StartPos.y / 2);
        endPos = new Vector2Int(createMap.Endpos.x / 2, createMap.Endpos.y / 2);
    }
}
using System.Collections.Generic;
using System.Linq;
using AI;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public MapReaderMono mapMono;
    private Dijkstra dijkstra;

    private List<Vector3> worldPos;

    public List<Vector3> WorldPos => worldPos;
    //public MapReaderMono mapMono => m_MapMono;

    private void Start()
    {
        mapMono = GetComponent<MapReaderMono>();
        CalculatePath();
    }

    private void CalculatePath()
    {
        dijkstra = new Dijkstra(mapMono.WayPoints);

        List<Vector2Int> vec2ToVec3 = dijkstra.FindPath(mapMono.StartPos, mapMono.EndPos).ToList();

        worldPos = new List<Vector3>();

        foreach (Vector2Int item in vec2ToVec3)
        {
            worldPos.Add(new Vector3(item.x * 2, 0.75f, item.y * 2));
        }
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace AI
{
public class Dijkstra : IPathFinder
{
    private readonly HashSet<Vector2Int> m_RightPosition;
    public Dijkstra(IEnumerable<Vector2Int> rightPosition)
    {
        m_RightPosition = new HashSet<Vector2Int>(rightPosition);
    }

    public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
    {
        Vector2Int currentNode = start;
        Dictionary<Vector2Int, Vector2Int?> ancestors = new Dictionary<Vector2Int, Vector2Int?>(){ { currentNode, null} };
        Queue<Vector2Int> frontier = new Queue<Vector2Int>(new[] { currentNode });

        while (frontier != null)
        {
            currentNode = frontier.Dequeue();

            if (currentNode == goal)
            {
                break;
            }

            foreach (Vector2Int direction in Tools.DirectionTools.Dirs)
            {
                Vector2Int next = currentNode + direction;

                if (m_RightPosition.Contains(next) && !ancestors.ContainsKey(next))
                {
                    frontier.Enqueue(next);
                    ancestors[next] = currentNode;
                }
            }
        }

        if (!ancestors.ContainsKey(goal))
        {
            return null;
        }

        List<Vector2Int> finalPath = new List<Vector2Int>();
        if (ancestors.ContainsKey(goal))
        {
            for (Vector2Int? run = goal; run.HasValue; run = ancestors[run.Value])
            {
                finalPath.Add(run.Value);
            }

            finalPath.Reverse();
            return finalPath;
        }

        return Enumerable.Empty<Vector2Int>();
    }
}    
}


//
// namespace AI
// {
// 	//TODO: Implement IPathFinder using Dijsktra algorithm.
// 	public class Dijkstra : IPathFinder
// 	{       
// 		public List<Vector2Int> listOfNodes;
// 		List<Vector2Int> path = new List<Vector2Int>();
// 		
// 		public Dijkstra(List<Vector2Int> _accesibles)
// 		{
// 			listOfNodes = _accesibles;
// 		}
//
// 		public IEnumerable<Vector2Int> FindPath(Vector2Int start, Vector2Int goal)
// 		{
// 			Vector2Int currentNode = start;
// 			
// 			Dictionary<Vector2Int, Vector2Int> ancestors = new Dictionary<Vector2Int, Vector2Int>() {{currentNode,default} };
// 			
// 			Queue<Vector2Int> frontier = new Queue<Vector2Int>(new[] {currentNode});
//
// 			while (frontier != null)
// 			{
// 				currentNode = frontier.Dequeue();
// 				
// 				if (currentNode == goal)
// 				{
// 					break;
// 				}
//
// 				foreach (Vector2Int direction in Tools.DirectionTools.Dirs)
// 				{
// 					Vector2Int next = currentNode + direction;
// 					
// 					if (listOfNodes.Contains(next))
// 					{
// 						if (!ancestors.ContainsKey(next))
// 						{
// 							frontier.Enqueue(next);
// 							ancestors.Add(next, currentNode);
// 						}		
// 					}
// 				}
// 			}
// 			
// 			if (!ancestors.ContainsKey(goal))
// 			{
// 				return null;
// 			}
// 			else if (ancestors.ContainsKey(goal))
// 			{
// 				for (int i = ancestors.Count - 1 ; i>=0; i--)
// 				{
// 					path.Add(currentNode);
// 					currentNode = ancestors.ElementAt(i).Value;
// 				}
// 			}
// 			return path;
// 		}
// 	}    
// }

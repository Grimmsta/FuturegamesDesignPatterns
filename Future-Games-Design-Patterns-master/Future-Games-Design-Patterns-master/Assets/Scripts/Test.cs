using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public GameObject prefab;
    
    private Pathfinding m_Pathfinding;
    private List<Vector3> path; 

    private void Awake()
    {
        m_Pathfinding = FindObjectOfType<Pathfinding>();
        path = m_Pathfinding.WorldPos;
        
        if (path != null)
        {
            print(path.Count);
        }
        else
        {
            print("welp");
        }
    }
}

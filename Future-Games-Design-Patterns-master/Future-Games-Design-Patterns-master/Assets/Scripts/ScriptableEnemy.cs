using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScriptableEnemy : ScriptableObject
{
    [SerializeField] private GameObject m_GameObject;
    [SerializeField] private int m_health;
    [SerializeField] private int m_speed;

    public int health => m_health;
    public int speed => m_speed;
}

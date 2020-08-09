using System.Collections.Generic;
using UnityEngine;
using Tools;

public class EnemyManager : MonoBehaviour
{
    private Pathfinding m_Path;
    private MapReaderMono m_Mono;
    private List<Vector3> m_List;
    private GameObjectPool m_SmallEnemyPool;
    private GameObjectPool m_BigEnemyPool;
    private int m_EnemiesToSpawn;
    private float m_ResetIntervall;
    private int m_EntetiesSpawned;
    private int m_Wave = 1;
    private int m_CurrentWave = 1;
    private int m_CurrentEnemy = 0;


    [SerializeField] private GameObject m_SmallEnemyPrefab;
    [SerializeField] private GameObject m_BigEnemyPrefab;
    [SerializeField] private float m_SpawnIntervall;

    private void Start()
    {
        m_Path = GetComponent<Pathfinding>();
        m_Mono = GetComponent<MapReaderMono>();

        m_List = m_Path.WorldPos;

        if (m_SmallEnemyPrefab != null)
        {
            m_SmallEnemyPool = new GameObjectPool(10, m_SmallEnemyPrefab);
        }

        if (m_BigEnemyPrefab != null)
        {
            m_BigEnemyPool = new GameObjectPool(10, m_BigEnemyPrefab);
        }

        m_ResetIntervall = m_SpawnIntervall;
    }

    private void Update()
    {
        InvokeRepeating("SpawnHandeler", 0, 10);
        m_SpawnIntervall -= Time.deltaTime;
    }

    private void SpawnHandeler()
    {
        foreach (string waveNum in m_Mono.MapCreate.Enemies)
        {
            print(m_CurrentWave);
            if (m_CurrentWave == m_Wave)
            {
                string[] seperator = { " " };
                string[] splitter = waveNum.Split(seperator, System.StringSplitOptions.RemoveEmptyEntries);

                foreach (string s in splitter)
                {
                    m_CurrentEnemy++;
                    m_EnemiesToSpawn = int.Parse(s);

                    CreateEnemy(m_EnemiesToSpawn, m_CurrentEnemy == 1 ? m_SmallEnemyPrefab : m_BigEnemyPrefab);
                }
            }
        }
    }

    private void CreateEnemy(int numberOfEntities, GameObject EnemyToSpawn)
    {
        for (int i = 0; i < numberOfEntities; i++)
        {
            if (m_SpawnIntervall <= 0 && m_EntetiesSpawned <= m_EnemiesToSpawn)
            {
                GameObject enemy = EnemyToSpawn == m_SmallEnemyPrefab ? m_SmallEnemyPool.Rent(true) : m_BigEnemyPool.Rent(true);
                enemy.transform.position = m_List[0];

                m_EntetiesSpawned++;
                m_SpawnIntervall = m_ResetIntervall;
            }
        }
    }
}

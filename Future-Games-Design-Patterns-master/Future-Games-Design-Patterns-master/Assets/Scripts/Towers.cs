using Tools;
using UnityEngine;
using System.Collections.Generic;

public class Towers : MonoBehaviour
{
    [SerializeField] private float m_ShootRange = 10;
    [SerializeField] private string m_Enemy_ID = "Enemy";
    [SerializeField] Transform m_TowerHead;
    [SerializeField] Transform m_Muzzle;
    [SerializeField] GameObject m_BulletPrefab;
    [SerializeField] private float m_fireRate = 1;
    [SerializeField] private float m_Countdown = 0;


    private GameObject[] m_Enemies;
    private GameObject m_ClosestEnemy = null;
    private GameObject m_Target = null;

    private GameObjectPool m_Bullets;

    public GameObject EnemyTarget => m_Target;

    private void Start()
    {
        InvokeRepeating("FindEnemies", 0, 1/60f);
        m_Bullets = new GameObjectPool(1, m_BulletPrefab);
    }

    private void FindEnemies()
    {
        m_Enemies = GameObject.FindGameObjectsWithTag(m_Enemy_ID);
        float shortestDistance = m_ShootRange;

        foreach (GameObject enemy in m_Enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if (distance <= shortestDistance)
            {
                shortestDistance = distance;
                m_ClosestEnemy = enemy;
            }
        }

        if (m_ClosestEnemy && shortestDistance < m_ShootRange)
        {
            m_Target = m_ClosestEnemy;
        }
        else
        {
            m_Target = null;
        }
    }

    private void Update()
    {
        if (m_Target == null)
        {
            return;
        }

        if (m_Countdown <= 0)
        {
            Shoot();
            m_Countdown = 1f / m_fireRate;
        }

        m_Countdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        if (m_Target)
        {
            Vector3 direction = m_Target.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);
            m_TowerHead.rotation = rotation;
        }

        GameObject bulletRent = m_Bullets.Rent(true);
        Bullet bullet = bulletRent.GetComponent<Bullet>();

        bullet.transform.position = m_Muzzle.transform.position;
        bullet.SeekTarget(m_Target);

    }

}

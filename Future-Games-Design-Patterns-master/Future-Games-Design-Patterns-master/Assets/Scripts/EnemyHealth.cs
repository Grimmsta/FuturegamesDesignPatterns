using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private ScriptableEnemy m_ScriptableObject;
    private int m_Health;

    void Awake()
    {
        m_Health = m_ScriptableObject.health;
    }
    // Update is called once per frame
    public void Damage(int damage)
    {
        m_Health -= damage;

        if (m_Health <=0)
        {
            gameObject.SetActive(false);
            m_Health = m_ScriptableObject.health;
            gameObject.GetComponent<EnemyMovement>().ResetUnit();
        }
    }
}

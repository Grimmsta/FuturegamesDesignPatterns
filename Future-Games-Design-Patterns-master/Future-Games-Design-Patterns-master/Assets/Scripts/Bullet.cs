using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int m_Damage;
    [SerializeField] private float m_Speed;
    [SerializeField] private int m_Enemy_ID;

    private GameObject m_Target;
    public void SeekTarget(GameObject target)
    {
        m_Target = target;
    }

    void Update()
    {
        if (m_Target.activeInHierarchy == false)
        {
            ResetUnit();
        }
        else
        {
            Vector3 dir = m_Target.transform.position - transform.position;
            transform.Translate(dir.normalized * m_Speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer.Equals(m_Enemy_ID))
        {
            if (m_Target != null)
            {
                m_Target.GetComponent<EnemyHealth>().Damage(m_Damage);
                ResetUnit();
            }
            else
            {
                return;
            }
        }
    }

    private void ResetUnit()
    {
        gameObject.SetActive(false);
        m_Target = null;
    }
}

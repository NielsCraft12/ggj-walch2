using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Scientists : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] float m_minRadius;
    [SerializeField] float m_maxRadius;
    [SerializeField] float m_waitTime;
    [Range(2, 10)] [SerializeField] int m_dropRate;
    [SerializeField] GameObject m_drop;

    int m_randomDrop;
    Vector3 m_startPos;
    NavMeshAgent m_agent;
    Vector3 m_targetPos;
    Coroutine m_setTarget;

    private void Start()
    {
        m_startPos = transform.position;
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.speed = m_speed;
    }

    private void Update()
    {
        if (!m_agent.pathPending && m_agent.remainingDistance <= m_agent.stoppingDistance)
        {
            if (m_setTarget == null)
            {
                m_setTarget = StartCoroutine(SetNewTarget());
            }
        }

        if(m_randomDrop == 1)
        {
            Instantiate(m_drop, transform.position, Quaternion.identity);
            m_randomDrop = 0;
        }
    }

    private IEnumerator SetNewTarget()
    {
        m_randomDrop = Random.Range(0, m_dropRate);
        yield return new WaitForSeconds(m_waitTime);
        m_targetPos = ChooseRandomPos();
        m_agent.SetDestination(m_targetPos);
        StopCoroutine(m_setTarget);
        m_setTarget = null;
    }

    private Vector3 ChooseRandomPos()
    {
        Vector2 _randomDirection = Random.insideUnitCircle.normalized;
        float _randomDistance = Random.Range(m_minRadius, m_maxRadius);
        Vector3 _randomPoint = m_startPos + new Vector3(_randomDirection.x, 0, _randomDirection.y) * _randomDistance;

        NavMeshHit _hit;
        if (NavMesh.SamplePosition(_randomPoint, out _hit, m_maxRadius, NavMesh.AllAreas))
        {
            return _hit.position;
        }

        return m_startPos;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            if (other.gameObject.GetComponent<DoorOpen>().Moving == false)
            {
                other.gameObject.GetComponent<DoorOpen>().Moving = true;
            }
        }
    }
}

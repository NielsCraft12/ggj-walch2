using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Scientists : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] float m_minRadius;
    [SerializeField] float m_maxRadius;
    [SerializeField] float m_waitTime;
    [Range(2, 10)] [SerializeField] int m_dropRate;
    [SerializeField] Animator m_messOmeterText;
    [SerializeField] GameObject m_drop;
    Animator m_animator;

    int m_randomDrop;
    Vector3 m_startPos;
    NavMeshAgent m_agent;
    Vector3 m_targetPos;
    Coroutine m_setTarget;

    private void Start()
    {
        m_startPos = transform.position;
        m_animator = GetComponentInChildren<Animator>();
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
            AudioManager.m_Instance.Play("Trash");
            Instantiate(m_drop, transform.position, Quaternion.identity);
            m_messOmeterText.SetTrigger("Shake");
            m_randomDrop = 0;
        }

        if (m_agent.velocity.magnitude < 0.1f)
        {
            AudioManager.m_Instance.Play("FootSteps");
            m_animator.SetBool("Walking", false);
        }else
            m_animator.SetBool("Walking", true);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Scientist"))
        {
            m_setTarget = null;
            if (m_setTarget == null)
            {
                m_setTarget = StartCoroutine(SetNewTarget());
            }
        }
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

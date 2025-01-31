using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Pesk : MonoBehaviour
{
    NavMeshAgent m_agent;

    private void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        StartCoroutine(RandomMovement());
    }

    private IEnumerator RandomMovement()
    {
        while (true)
        {
            Vector3 _randomDestination = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));

            m_agent.SetDestination(_randomDestination);

            float _randomSpeed = Random.Range(4f, 8f);
            m_agent.speed = _randomSpeed;

            float _waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(_waitTime);
        }
    }
}

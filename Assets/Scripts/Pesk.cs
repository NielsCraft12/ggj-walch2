using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Pesk : MonoBehaviour
{
    NavMeshAgent m_agent;
    [SerializeField] GameObject m_trash;
    [Range(2, 10)] [SerializeField] int m_dropRate;

    private void Start()
    {
        AudioManager.m_Instance.Play("Rat");
        m_agent = GetComponent<NavMeshAgent>();
        StartCoroutine(RandomMovement());
    }

    private IEnumerator RandomMovement()
    {
        while (true)
        {
            Vector3 _randomDestination = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));

            if(m_agent != null )
            {
                m_agent.SetDestination(_randomDestination);
            }

            float _randomSpeed = Random.Range(4f, 8f);
            m_agent.speed = _randomSpeed;

            float _waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(_waitTime);
            int _dropChance = Random.Range(0, m_dropRate);
            if(_dropChance == 1)
            {
                AudioManager.m_Instance.Play("Trash");
                Instantiate(m_trash, transform.position, Quaternion.identity);
                _dropChance = 0;
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

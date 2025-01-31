using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("Time")]
    public const int m_hoursInDay = 24, m_minutesInHour = 60;
    [HideInInspector] public float m_dayDuration;

    float m_totalTime = 0;
    float m_currentTime = 0;

    [Header("Other")]
    [SerializeField] Slider m_messOmeter;
    List<GameObject> m_allTrash;

    private void Start()
    {
        m_dayDuration = SettingsSingleton.Instance.settings.m_GameTime;

        if(m_allTrash == null)
        {
            m_allTrash = new List<GameObject>();
        }
    }

    private void Update()
    {
        m_totalTime += Time.deltaTime;
        m_currentTime = m_totalTime % m_dayDuration;

        GameObject[] _trashObjects = GameObject.FindGameObjectsWithTag("Trash");
        m_allTrash.Clear();
        m_allTrash.AddRange(_trashObjects);
        m_messOmeter.value = m_allTrash.Count;
    }

    public float GetHour()
    {
        return m_currentTime * m_hoursInDay / m_dayDuration;
    }

    public float GetMinutes()
    {
        return (m_currentTime * m_hoursInDay * m_minutesInHour / m_dayDuration) % m_minutesInHour;
    }

    public string Clock24Hour()
    {
        return Mathf.FloorToInt(GetHour()).ToString("00") + ":" + Mathf.FloorToInt(GetMinutes()).ToString("00");
    }
}

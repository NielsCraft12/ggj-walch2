using UnityEngine;

public class UiManager : MonoBehaviour
{
    public const int m_hoursInDay = 24, m_minutesInHour = 60;
    public float m_dayDuration;

    float m_totalTime = 0;
    float m_currentTime = 0;

    private void Start()
    {
        m_dayDuration = SettingsSingleton.Instance.settings.m_GameTime;
    }

    private void Update()
    {
        m_totalTime += Time.deltaTime;
        m_currentTime = m_totalTime % m_dayDuration;
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

    public string Clock12Hour()
    {
        int _hour = Mathf.FloorToInt(GetHour());
        string _abbrevation = "AM";

        if (_hour >= 12)
        {
            _abbrevation = "PM";
            _hour -= 12;
        }

        if (_hour == 0) _hour = 12;

        return _hour.ToString("00") + ":" + Mathf.FloorToInt(GetMinutes()).ToString("00") + " " + _abbrevation;
    }
}

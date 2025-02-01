using UnityEngine;

public class ClockScript : MonoBehaviour
{
    UiManager uiManager;

    public RectTransform m_minPointer;
    public RectTransform m_hourPointer;
    [SerializeField] Animator m_warningAni;

    const float m_hoursToDegrees = 360 / 12, m_minutesToDegrees = 360 / 60;

    private void Start()
    {
        uiManager = FindFirstObjectByType<UiManager>();
    }

    private void Update()
    {
        m_hourPointer.rotation = Quaternion.Euler(0, 0, -uiManager.GetHour()*m_hoursToDegrees);
        m_minPointer.rotation = Quaternion.Euler(0, 0, -uiManager.GetMinutes() * m_minutesToDegrees);

        if (uiManager.m_currentTime >= (uiManager.m_dayDuration / 12 * 11))
            m_warningAni.SetTrigger("StartPulsing");
    }
}

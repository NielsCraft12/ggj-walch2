using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] float m_gameTime;

    private void Start()
    {
        SettingsSingleton.Instance.settings.m_GameTime = 0;
    }

    private void Update()
    {
        SettingsSingleton.Instance.settings.m_GameTime += Time.deltaTime * m_gameTime;
    }
}

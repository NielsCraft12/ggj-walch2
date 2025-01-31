using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] UiManager m_uiManager;
    [SerializeField] GameObject m_rat;
    [SerializeField] Transform m_ratSpawn;
    List<GameObject> m_rats = new List<GameObject>();

    private void Start()
    {
        SettingsSingleton.Instance.settings.m_IsPaused = false;
    }

    private void Update()
    {
        if (SettingsSingleton.Instance.settings.m_IsPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        if (m_uiManager.m_totalTime >= SettingsSingleton.Instance.settings.m_GameTime)
        {
            SceneManager.LoadScene("GameOver");
            AudioManager.m_Instance.StopAllSounds();
            AudioManager.m_Instance.Play("LevelEnd");
        }

        if (m_uiManager.m_allTrash.Count >= 5 && m_rats.Count < m_uiManager.m_allTrash.Count / 5)
        {
            GameObject _newRat = Instantiate(m_rat, m_ratSpawn.position, Quaternion.identity);
            m_rats.Add(_newRat);
        }
    }
}

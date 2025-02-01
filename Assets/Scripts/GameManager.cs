using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UiManager m_uiManager;
    [SerializeField] GameObject m_rat;
    [SerializeField] List<Transform> m_ratSpawns;
    List<GameObject> m_rats = new List<GameObject>();
    public int m_CleanedCount;

    private int m_LastTrashCount = 0;

    private void Start()
    {
        m_CleanedCount = 0;
        SettingsSingleton.Instance.settings.m_IsPaused = false;
        m_LastTrashCount = 0;
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
            SettingsSingleton.Instance.settings.m_Grade = CalculateGrade();
            SceneManager.LoadScene("GameOver");
            AudioManager.m_Instance.StopAllSounds();
            AudioManager.m_Instance.Play("LevelEnd");
        }

        HandleRatSpawning();
    }

    private void HandleRatSpawning()
    {
        int currentTrashCount = m_uiManager.m_allTrash.Count;

        if (currentTrashCount >= 5)
        {
            int expectedRatCount = currentTrashCount / 5;

            while (m_rats.Count < expectedRatCount)
            {
                GameObject _newRat = Instantiate(m_rat, m_ratSpawns[Random.Range(0, m_ratSpawns.Count)].position, Quaternion.identity);
                m_rats.Add(_newRat);
            }
        }

        if (currentTrashCount < 5)
        {
            foreach (GameObject rat in m_rats)
            {
                Destroy(rat);
            }
            m_rats.Clear();
        }

        m_LastTrashCount = currentTrashCount;
    }

    private string CalculateGrade()
    {
        int _totalTrash = m_uiManager.m_allTrash.Count + m_CleanedCount;
        int _cleanedTrash = m_CleanedCount;

        if (_totalTrash == 0) return "F";

        float _score = (_cleanedTrash / (float)_totalTrash) * 100;

        if (_score >= 90) return "A+";
        if (_score >= 85) return "A";
        if (_score >= 80) return "A-";
        if (_score >= 75) return "B+";
        if (_score >= 70) return "B";
        if (_score >= 65) return "B-";
        if (_score >= 60) return "C+";
        if (_score >= 55) return "C";
        if (_score >= 50) return "C-";
        if (_score >= 40) return "D";
        return "F";
    }
}
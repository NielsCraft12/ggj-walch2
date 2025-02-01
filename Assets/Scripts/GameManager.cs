using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] UiManager m_uiManager;
    [SerializeField] GameObject m_rat;
    [SerializeField] Transform m_ratSpawn;
    List<GameObject> m_rats = new List<GameObject>();
    public int m_CleanedCount;
    private const float m_gradeScaling = 150f;

    private void Start()
    {
        m_CleanedCount = 0;
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
            SettingsSingleton.Instance.settings.m_Grade = CalculateGrade();
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

    private string CalculateGrade()
    {
        int _totalTrash = m_uiManager.m_allTrash.Count + m_CleanedCount;
        int _cleanedTrash = m_CleanedCount;

        if (_totalTrash == 0) return "F";

        float _score = (_cleanedTrash / (float)_totalTrash) * 100;
        float _adjustmentFactor = 1 - Mathf.Exp(-_totalTrash / m_gradeScaling);
        float _finalScore = _score * _adjustmentFactor;

        if (_finalScore >= 90) return "A+";
        if (_finalScore >= 85) return "A";
        if (_finalScore >= 80) return "A-";
        if (_finalScore >= 75) return "B+";
        if (_finalScore >= 70) return "B";
        if (_finalScore >= 65) return "B-";
        if (_finalScore >= 60) return "C+";
        if (_finalScore >= 55) return "C";
        if (_finalScore >= 50) return "C-";
        if (_finalScore >= 40) return "D";
        return "F";
    }
}

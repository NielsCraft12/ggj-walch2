using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.05f, 1.05f);
        AudioManager.m_Instance.Play("ButtonHover");
    }

    public void PointerExit()
    {
        transform.localScale = Vector2.one;
    }

    public void StartGame()
    {
        AudioManager.m_Instance.Play("ButtonClick");
        AudioManager.m_Instance.Play("Music");
        SceneManager.LoadScene("GameView");
    }

    public void MainMenu()
    {
        AudioManager.m_Instance.Play("ButtonClick");
        AudioManager.m_Instance.StopAllSounds();
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        AudioManager.m_Instance.Play("ButtonClick");
        SettingsSingleton.Instance.settings.m_IsPaused = false;
    }

    public void QuitGame()
    {
        AudioManager.m_Instance.Play("ButtonClick");

        #if (UNITY_EDITOR || DEVELOPMENT_BUILD)
            Debug.Log(this.name + " : " + this.GetType() + " : " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        #endif
        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE)
             Application.Quit();
        #elif (UNITY_WEBGL)
             Application.OpenURL("https://joahvds.itch.io/walch-origins");
        #endif
    }
}

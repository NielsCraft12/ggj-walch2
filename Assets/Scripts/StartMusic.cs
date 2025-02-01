using UnityEngine;

public class StartMusic : MonoBehaviour
{
    private void Start()
    {
        AudioManager.m_Instance.Play("Music");
    }
}

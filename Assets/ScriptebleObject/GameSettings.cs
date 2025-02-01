using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public bool m_Audio;
    public bool m_Music;
    public float m_GameTime;
    public bool m_IsPaused;
    public string m_Grade;
}

using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [Header("Texts")]
    [SerializeField] TextMeshProUGUI m_gameTimeText;

    private void Update()
    {
        float _roundedTime = Mathf.Ceil(SettingsSingleton.Instance.settings.m_GameTime);
        m_gameTimeText.text = _roundedTime.ToString();
    }
}

using TMPro;
using UnityEngine;

public class SetGrade : MonoBehaviour
{
    private void Start()
    {
        TextMeshProUGUI _myself = GetComponent<TextMeshProUGUI>();
        _myself.text = SettingsSingleton.Instance.settings.m_Grade;
    }
}

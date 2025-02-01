using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButtons : MonoBehaviour
{
    [SerializeField] Button m_audioButton;
    [SerializeField] Button m_musicButton;

    [SerializeField] List<Sprite> m_images;

    [SerializeField] bool m_isAudioButton;

    private void Start()
    {
        if (m_isAudioButton)
        {
            UpdateButtonSprite(m_audioButton, SettingsSingleton.Instance.settings.m_Audio);
        }
        else
        {
            UpdateButtonSprite(m_musicButton, SettingsSingleton.Instance.settings.m_Music);
        }
    }

    public void PointerEnter()
    {
        transform.localScale = new Vector2(1.05f, 1.05f);
        AudioManager.m_Instance.Play("ButtonHover");
    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(1f, 1f);
    }

    public void AudioButton()
    {
        if (m_audioButton != null)
        {
            AudioManager.m_Instance.StopAllSounds();
            AudioManager.m_Instance.Play("ButtonClick");
            SettingsSingleton.Instance.settings.m_Audio = !SettingsSingleton.Instance.settings.m_Audio;
            UpdateButtonSprite(m_audioButton, SettingsSingleton.Instance.settings.m_Audio);

        }
    }

    public void MusicButton()
    {
        if (m_musicButton != null)
        {
            if (SettingsSingleton.Instance.settings.m_Music)
            {
                AudioManager.m_Instance.StopAllSounds();
            }

            AudioManager.m_Instance.Play("ButtonClick");

            SettingsSingleton.Instance.settings.m_Music = !SettingsSingleton.Instance.settings.m_Music;
            UpdateButtonSprite(m_musicButton, SettingsSingleton.Instance.settings.m_Music);

            if (SettingsSingleton.Instance.settings.m_Music)
            {
                AudioManager.m_Instance.Play("Music");
            }
        }
    }

    private void UpdateButtonSprite(Button _button, bool _isActive)
    {
        if (_button != null && m_images != null && m_images.Count >= 2)
        {
            _button.image.sprite = _isActive ? m_images[1] : m_images[0];
        }
    }
}

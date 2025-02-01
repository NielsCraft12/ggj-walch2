using System.Collections.Generic;
using UnityEngine;

public class Lightmanager : MonoBehaviour
{
    [SerializeField]
    float lightsOnTime;

    [SerializeField]
    List<Light> lights = new List<Light>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    UiManager uiManager;

    void Start()
    {
        uiManager = FindAnyObjectByType<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (uiManager.m_currentTime >= lightsOnTime)
        {
            foreach (Light light in lights)
            {
                light.enabled = true;
            }
        }
        else
        {
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
        }
    }
}

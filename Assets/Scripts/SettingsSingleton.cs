using UnityEngine;

public class SettingsSingleton : MonoBehaviour
{
    private static SettingsSingleton instance;

    public static SettingsSingleton Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<SettingsSingleton>();

                if (instance == null)
                {
                    GameObject singletonObject = new GameObject("SettingsSingleton");
                    instance = singletonObject.AddComponent<SettingsSingleton>();
                }
            }
            return instance;
        }
    }

    public GameSettings settings;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

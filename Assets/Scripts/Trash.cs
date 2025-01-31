using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    [SerializeField] List<GameObject> m_TrashPiles;

    private void Start()
    {
        foreach(GameObject _trash in m_TrashPiles)
        {
            _trash.SetActive(false);
        }

        int _randomPile = Random.Range(0, 3);
        Debug.Log(_randomPile);
        m_TrashPiles[_randomPile].SetActive(true);
    }
}

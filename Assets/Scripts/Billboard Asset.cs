using UnityEngine;

public class BillboardAsset : MonoBehaviour
{
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        transform.LookAt(mainCamera.transform, Vector3.up);
        //transform.Rotate(0, 180, 0);
    }
}

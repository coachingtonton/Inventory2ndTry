using UnityEngine;

public class ReflectionCameraFollow : MonoBehaviour
{
    public Camera mainCam;

    void LateUpdate()
    {
        transform.position = mainCam.transform.position;
        transform.rotation = mainCam.transform.rotation;
    }
}
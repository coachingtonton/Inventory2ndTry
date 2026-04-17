using UnityEngine;

public class FirePointRotator : MonoBehaviour
{
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = transform.parent; // assumes GunPivot is direct child of Player
    }

    private void Update()
    {
        // follow player position
        transform.position = playerTransform.position;

        // rotate toward mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
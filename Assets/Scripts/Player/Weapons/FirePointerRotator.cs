using UnityEngine;
using System;

public class FirePointRotator : MonoBehaviour
{
    [SerializeField] private Vector3 firePointOffset = new Vector3(0f, 0.5f, 0f);
    [SerializeField] private Transform playerTransform;
    MeleeHandler meleeHandler;

    private void Awake()
    {
        meleeHandler = FindFirstObjectByType<MeleeHandler>();
    }

    private void Start()
    {
        // GOT RID OF THE PARENT,
        // Gun being a child was causing spawn projectile 
        //flipping issues 
        transform.SetParent(null);
    }

    private void Update()
    {
        // Follow player position, added offset to account for player sprite
        transform.position = playerTransform.position + firePointOffset;

        if (!meleeHandler.isCurrentlySwinging)
        {
            // Rotate toward mouse
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = mousePos - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Flip gun sprite so it doesn't appear upside-down when aiming left
            Vector3 scale = transform.localScale;
            scale.y = (angle > 90f || angle < -90f) ? -1f : 1f;
            transform.localScale = scale;
        }
    }
}
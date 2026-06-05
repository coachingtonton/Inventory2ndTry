using UnityEngine;

public class FloorReflection : MonoBehaviour
{
    [SerializeField] SpriteRenderer source;   // the character's renderer
    [SerializeField] float opacity = 0.15f;
    SpriteRenderer sr;

    void Awake() => sr = GetComponent<SpriteRenderer>();

    void LateUpdate()
    {
        sr.sprite = source.sprite;            // match current animation frame
        sr.flipX = source.flipX;             // match facing direction
        sr.color = new Color(0f, 0f, 0f, 0f) // tint toward floor color
                  + new Color(1, 1, 1, opacity) * 0.6f;
    }
}
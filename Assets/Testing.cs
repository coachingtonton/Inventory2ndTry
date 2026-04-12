using UnityEngine;

public class Testing : MonoBehaviour 
{
    private Rigidbody2D rb;

    private void Start()
    {
           rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (InputManager.Instance.bKeyPressed)
        {
            Debug.Log("FORCE MODE");
            rb.AddForce(Vector2.right* 10f, ForceMode2D.Force);
        }
        if (InputManager.Instance.zKeyPressed)
        {
            Debug.Log("IMPULSE");
            rb.AddForce(Vector2.up * 10f, ForceMode2D.Impulse);
        }
        if (InputManager.Instance.xKeyPressed)
        {
            Debug.Log("LINEARVELOCITY");
            rb.linearVelocity = new Vector2(5f, rb.linearVelocity.y);
        }
    }

}

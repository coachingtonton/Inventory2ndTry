using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GameObject firepoint;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );//Converts mouse position to coordinates in the scene
        mousePos.z = 0;// Game is 2D no need to worry about position 

        Vector2 direction = mousePos - transform.position;// gets directions to where the mouse is in relation to PARENT OBJECT AKA THE FIREPOINT 
        //gives a vector from player to where mouse is inside the scene 

        float angle = Mathf.Atan2 ( direction.x, direction.y ) * Mathf.Rad2Deg;///Converts direction's x and y into a degree
        ///IF MOUSE IOS DIRECLY RIGHT ANGLE IS 0 IF MOUSE IS UP 90 AND ETC 
        
        firepoint.transform.rotation = Quaternion.Euler(0f, 0f, angle);// TAKES firepoint and SETS IT TO THE ANGLE 
        // ALONG THE Z AXIS

    }
}

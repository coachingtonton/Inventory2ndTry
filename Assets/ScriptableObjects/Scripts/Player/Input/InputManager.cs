using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public float moveInput;
    public bool jumpPressed;
    public bool dashPressed;
    public bool firePressed;
    public bool fireHeld;
    public bool interactPressed;
    public bool pausePressed;
    public bool gravityFlipPressed;
    public bool rPressed;

    public bool iKeyPressed;
    public bool zKeyPressed;
    public bool xKeyPressed;
    public bool cKeyPressed;
    public bool vKeyPressed;
    public bool bKeyPressed;



    public bool onePressed;
    public bool twoPressed;
    public bool threePressed;
    public bool fourPressed;
    public bool fivePressed;
    public bool sixPressed;
    public bool sevenPressed;
    public bool eightPressed;
    public bool ninePressed;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        dashPressed = Input.GetKeyDown(KeyCode.LeftShift);
        firePressed = Input.GetMouseButtonDown(0);
        fireHeld = Input.GetMouseButton(0);
        interactPressed = Input.GetKeyDown(KeyCode.E);
        pausePressed = Input.GetKeyDown(KeyCode.Escape);
        gravityFlipPressed = Input.GetKeyDown(KeyCode.G);
        rPressed = Input.GetKeyDown(KeyCode.R);
        
        iKeyPressed = Input.GetKeyDown(KeyCode.I);
        zKeyPressed = Input.GetKeyDown(KeyCode.Z);
        xKeyPressed = Input.GetKeyDown(KeyCode.X);
        cKeyPressed = Input.GetKeyDown(KeyCode.C);
        vKeyPressed = Input.GetKeyDown(KeyCode.V);
        bKeyPressed = Input.GetKeyDown(KeyCode.B);


        onePressed = Input.GetKeyDown(KeyCode.Alpha1);
        twoPressed = Input.GetKeyDown(KeyCode.Alpha2);
        threePressed = Input.GetKeyDown(KeyCode.Alpha3);
        fourPressed = Input.GetKeyDown(KeyCode.Alpha4);
        fivePressed = Input.GetKeyDown(KeyCode.Alpha5);
        sixPressed = Input.GetKeyDown(KeyCode.Alpha6);
        sevenPressed = Input.GetKeyDown(KeyCode.Alpha7);
        eightPressed = Input.GetKeyDown(KeyCode.Alpha8);
        ninePressed = Input.GetKeyDown(KeyCode.Alpha9);
    }
}
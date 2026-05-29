using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [Header("Movement")]
    public float moveInput;
    public float moveInputY;
    public bool jumpPressed;
    public bool dashPressed;
    public bool spaceHeld;
    public bool wKeyHeld;
    public bool wKeyPressed;

    [Header("Combat")]
    public bool firePressed;
    public bool fireHeld;
    public bool fireReleased;
    public bool rightClickPressed;
    public bool rightClickReleased;

    public bool rightClickHeld;
    public bool rPressed;
    public bool rKeyPressed;
    public bool cntrlPressed;

    [Header("Interaction")]
    public bool interactPressed;
    public bool eKeyPressed;
    public bool gravityFlipPressed;

    [Header("UI")]
    public bool pausePressed;
    public bool iKeyPressed;
    public bool tabHeld;

    [Header("Hotkeys")]
    public bool zKeyPressed;
    public bool xKeyPressed;
    public bool cKeyPressed;
    public bool vKeyPressed;
    public bool bKeyPressed;
    public bool bkeyHeld;

    [Header("Number Keys")]
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
        // Movement
        moveInput = Input.GetAxisRaw("Horizontal");
        moveInputY = Input.GetAxisRaw("Vertical");
        jumpPressed = Input.GetKeyDown(KeyCode.Space);
        spaceHeld = Input.GetKey(KeyCode.Space);
        dashPressed = Input.GetKeyDown(KeyCode.LeftShift);
        wKeyPressed = Input.GetKeyDown(KeyCode.W);
        wKeyHeld = Input.GetKey(KeyCode.W);

        // Combat
        firePressed = Input.GetMouseButtonDown(0);
        fireHeld = Input.GetMouseButton(0);
        rightClickReleased = Input.GetMouseButtonUp(1);
        fireReleased = Input.GetMouseButtonUp(0);
        rightClickPressed = Input.GetMouseButtonDown(1);
        rightClickHeld = Input.GetMouseButton(1);
        rPressed = Input.GetKeyDown(KeyCode.R);
        rKeyPressed = Input.GetKeyUp(KeyCode.R);
        cntrlPressed = Input.GetKeyDown(KeyCode.LeftControl);

        // Interaction
        eKeyPressed = Input.GetKeyDown(KeyCode.E);
        interactPressed = Input.GetKeyDown(KeyCode.E);
        gravityFlipPressed = Input.GetKeyDown(KeyCode.G);

        // UI
        pausePressed = Input.GetKeyDown(KeyCode.Escape);
        iKeyPressed = Input.GetKeyDown(KeyCode.I);
        tabHeld = Input.GetKey(KeyCode.Tab);

        // Hotkeys
        zKeyPressed = Input.GetKeyDown(KeyCode.Z);
        xKeyPressed = Input.GetKeyDown(KeyCode.X);
        cKeyPressed = Input.GetKeyDown(KeyCode.C);
        vKeyPressed = Input.GetKeyDown(KeyCode.V);
        bKeyPressed = Input.GetKeyDown(KeyCode.B);
        bkeyHeld = Input.GetKey(KeyCode.B);

        // Number Keys
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
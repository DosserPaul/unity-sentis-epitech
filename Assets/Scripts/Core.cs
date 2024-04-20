using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject player;
    public Texture2D circleCursor;

    public enum MODE { WALK, CONTROL };
    public MODE mode = MODE.WALK;

    private CharacterController controller;
    private float speed = 8;

    public static Game instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        SetupController();
        SetupCursor();
    }

    private void SetupController()
    {
        controller = player.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void SetupCursor()
    {
        if (mode == MODE.CONTROL)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.SetCursor(circleCursor, new Vector2(16, 16), CursorMode.Auto);
        }
    }

    private void Update()
    {
        HandlePlayerMovement();
        HandleMouseInput();
        HandleGameModeToggle();
    }

    private void HandlePlayerMovement()
    {
        float vert = Input.GetAxis("Vertical");
        float horiz = Input.GetAxis("Horizontal");
        float mouseX = Input.GetAxis("Mouse X");

        float factor = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? 2 : 1;

        controller.Move((player.transform.forward * vert + player.transform.right * horiz) * Time.deltaTime * speed * factor);

        if (mode == MODE.WALK)
        {
            controller.transform.Rotate(Vector3.up, mouseX);
        }
    }

    private void HandleMouseInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleGameMode();
        }
    }

    private void ToggleGameMode()
    {
        switch (mode)
        {
            case MODE.WALK:
                mode = MODE.CONTROL;
                SetupCursor();
                break;
            case MODE.CONTROL:
                mode = MODE.WALK;
                Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }

    private void HandleGameModeToggle()
    {
        if (mode == MODE.CONTROL)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}

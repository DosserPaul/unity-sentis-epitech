using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float pushPower = 2.0f;
    [SerializeField] private float jumpSpeed = 8.0f;
    [SerializeField] private float gravity = 20.0f;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveHorizontal, 0.0f, moveVertical);
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= pushPower;

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (characterController.isGrounded && Input.GetButtonDown("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (!IsValidRigidbody(body, hit.moveDirection))
        {
            return;
        }

        Vector3 pushDir = CalculatePushDirection(hit.moveDirection);
        ApplyPushForce(body, pushDir);
    }

    private bool IsValidRigidbody(Rigidbody body, Vector3 moveDirection)
    {
        return body != null && !body.isKinematic && moveDirection.y >= -0.3f;
    }

    private Vector3 CalculatePushDirection(Vector3 moveDirection)
    {
        return new Vector3(moveDirection.x, 0, moveDirection.z);
    }

    private void ApplyPushForce(Rigidbody body, Vector3 pushDir)
    {
        body.velocity = pushDir * pushPower;
    }
}

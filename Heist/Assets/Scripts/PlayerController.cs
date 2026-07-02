using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference lookAction;
    [SerializeField] InputActionReference sprintAction;
    [SerializeField] float walkSpeed;
    [SerializeField] float sprintSpeed;
    [SerializeField] float mouseSensitivity;

    float stamina = 100f;

    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private bool sprinting;

    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveDirection = moveAction.action.ReadValue<Vector2>();
        lookDirection = lookAction.action.ReadValue<Vector2>();
        sprinting = sprintAction.action.IsPressed();

        stamina += sprinting && moveDirection != Vector2.zero ? -Time.deltaTime * 10 : Time.deltaTime * 10;
        stamina = Mathf.Clamp(stamina, 0f, 100f);

        pitch -= lookDirection.y * (Mathf.Clamp(mouseSensitivity - 0.2f, 0.1f, 1f));
        pitch = Mathf.Clamp(pitch, -90f, 90f);
 
        playerTransform.Rotate(0, lookDirection.x * mouseSensitivity, 0);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    void FixedUpdate()
    {
        float frameSpeed = sprinting && stamina > 0f ? sprintSpeed : walkSpeed;

        Vector3 movement = playerTransform.forward * moveDirection.y + playerTransform.right * moveDirection.x;
        rigidBody.linearVelocity = new Vector3(movement.x * frameSpeed, rigidBody.linearVelocity.y, movement.z * frameSpeed);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraPivot;
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] InputActionReference moveAction;
    [SerializeField] InputActionReference lookAction;
    [SerializeField] float walkSpeed;
    [SerializeField] float mouseSensitivity;

    private Vector2 moveDirection;
    private Vector2 lookDirection;
    private float pitch = 0f;

    void Update()
    {
        moveDirection = moveAction.action.ReadValue<Vector2>();
        lookDirection = lookAction.action.ReadValue<Vector2>();

        pitch -= lookDirection.y * (mouseSensitivity - 0.2f);
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        playerTransform.Rotate(0, lookDirection.x * mouseSensitivity, 0);
        cameraPivot.localRotation = Quaternion.Euler(pitch, 0, 0);
    }

    void FixedUpdate()
    {
        Vector3 movement = playerTransform.forward * moveDirection.y + playerTransform.right * moveDirection.x;
        rigidBody.linearVelocity = new Vector3(movement.x * walkSpeed, rigidBody.linearVelocity.y, movement.z * walkSpeed);
    }
}

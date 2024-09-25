using UnityEngine.InputSystem;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    [SerializeField]
    [Min(1)]
    float speed;

    PlayerInput playerInput;
    Vector2 movement;
    Rigidbody2D body;

    void Awake() {
        playerInput = new();
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        playerInput.Paddle.Move.performed += Move;
        playerInput.Enable();
    }

    void OnDisable() {
        playerInput.Disable();
    }

    void Move(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();
        body.velocity = movement * speed;
    }
}

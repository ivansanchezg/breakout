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

    float minX = -7.33f;
    float maxX = 7.33f;

    void Awake() {
        playerInput = new();
        body = GetComponent<Rigidbody2D>();
    }

    void OnEnable() {
        playerInput.Paddle.Move.performed += Move;
        playerInput.Enable();
    }

    void OnDisable() {
        playerInput.Paddle.Move.performed -= Move;
        playerInput.Disable();
    }

    void Move(InputAction.CallbackContext context) {
        movement = context.ReadValue<Vector2>();
    }

    void FixedUpdate() {
        var newPosition = body.position + (speed * Time.fixedDeltaTime * movement);
        var newX = Mathf.Clamp(newPosition.x, minX, maxX);
        body.MovePosition(new Vector2(newX, newPosition.y));
    }
}

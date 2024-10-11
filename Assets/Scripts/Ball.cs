using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    BoxCollider2D boxCollider;
    Rigidbody2D body;
    Vector2 velocity;

    readonly float speed = 5f;
    readonly float startMagnitude = 6f;
    readonly float speedIncrement = 0.25f;

    float magnitude;
    int bounces;
    float extraSpeed;
    float magnitudeIncrement;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Start() {
        bounces = 0;
        extraSpeed = 0f;
        magnitude = startMagnitude;
        magnitudeIncrement = Mathf.Sqrt(Mathf.Pow(speedIncrement, 2f) * 2);
        body.velocity = Vector2.zero;
        velocity = body.velocity;
        StartCoroutine(SetVelocity());
    }

    IEnumerator SetVelocity() {
        yield return new WaitForSeconds(1);
        int x;
        do {
            x = Random.Range((int) -speed, (int) speed + 1);
        } while (x == 0);
        body.velocity = new Vector2(x, calculateY(x) * -1f);        
        velocity = body.velocity;
    }

    float calculateY(float x) {
        return Mathf.Sqrt((magnitude * magnitude) - (x * x));
    }

    void OnCollisionEnter2D(Collision2D collision) {
        bounces += 1;

        if (bounces % 10 == 0) {
            extraSpeed += speedIncrement;
            magnitude += magnitudeIncrement;
        }

        if (collision.gameObject.CompareTag("Wall")) {
            body.velocity = velocity * new Vector2(-1, 1);
            velocity = body.velocity;
        } 
        
        if (collision.gameObject.CompareTag("Ceiling")) {
            body.velocity = velocity * new Vector2(1, -1);
            velocity = body.velocity;

        }
        
        if (collision.gameObject.CompareTag("Paddle")) {
            // var contactPoint = collision.contacts[0].point;
            var ballBounds = boxCollider.bounds;
            var paddleBoxCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            var paddleBounds = paddleBoxCollider.bounds;

            var xDiff = Mathf.Abs(ballBounds.center.x - paddleBounds.center.x);
            var diffPercentage = xDiff / 0.67f;
            var x = Mathf.Clamp((speed * diffPercentage) + extraSpeed, 1.5f + extraSpeed, speed + extraSpeed);

            if (ballBounds.center.x >= paddleBounds.center.x) {         // Touched right side
                body.velocity = new Vector2(x, calculateY(x));
            } else {                                                    // Touched left side
                body.velocity = new Vector2(x * -1, calculateY(x));
            }
            velocity = body.velocity;
        }
        
        if (collision.gameObject.CompareTag("Brick")) {
            var brickContactPoint = collision.contacts[0].point;
            var brickBoxCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            var brickBounds = brickBoxCollider.bounds;

            if (Approximately(brickContactPoint.x, brickBounds.min.x)) {            // Left side of the brick
                body.velocity = velocity * new Vector2(-1, 1);
            } else if (Approximately(brickContactPoint.x, brickBounds.max.x)) {     // Right side of the brick
                body.velocity = velocity * new Vector2(-1, 1);
            } else if (Approximately(brickContactPoint.y, brickBounds.min.y)) {     // Bottom side of the brick                
                body.velocity = velocity * new Vector2(1, -1);
            } else if (Approximately(brickContactPoint.y, brickBounds.max.y)) {     // Top side of the brick
                body.velocity = velocity * new Vector2(1, -1);
            }
            velocity = body.velocity;

            var brick = collision.gameObject.GetComponent<Brick>();
            GameManager.instance.DestroyBrick(brick);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Floor")) {
            GameManager.instance.LoseLive();
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("OutOfBounds")) {
            Reset();
        }
    }

    void Reset() {
        velocity = Vector2.zero;
        body.MovePosition(new Vector2(0, 1));
        StartCoroutine(SetVelocity());
    }

    bool Approximately(float valueA, float valueB) {
        float diff = Mathf.Abs(valueA - valueB);
        return diff <= 0.01f;
    }
}
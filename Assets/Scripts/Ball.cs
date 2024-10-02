using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D body;
    Vector2 velocity;

    void Awake() {
        body = GetComponent<Rigidbody2D>();
    }

    void Start() {
        body.velocity = Vector2.zero;
        velocity = body.velocity;
        StartCoroutine(SetVelocity());
    }

    IEnumerator SetVelocity() {
        yield return new WaitForSeconds(1);
        int x;
        do {
            x = Random.Range(-5, 6);
        } while (x == 0);
        body.velocity = new Vector2(x, -4);
        velocity = body.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Wall")) {
            body.velocity = velocity * new Vector2(-1, 1);
            velocity = body.velocity;
        } else if (collision.gameObject.CompareTag("Ceiling")) {
            body.velocity = velocity * new Vector2(1, -1);
            velocity = body.velocity;
        } else if (collision.gameObject.CompareTag("Paddle")) {
            body.velocity = velocity * new Vector2(-1, -1);
            velocity = body.velocity;
        } else if (collision.gameObject.CompareTag("Brick")) {
            var contactPoint = collision.contacts[0].point;
            var boxCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            var bounds = boxCollider.bounds;

            if (Approximately(contactPoint.x, bounds.min.x)) {
                body.velocity = velocity * new Vector2(-1, 1);
            } else if (Approximately(contactPoint.x, bounds.max.x)) {
                body.velocity = velocity * new Vector2(-1, 1);
            } else if (Approximately(contactPoint.y, bounds.min.y)) {
                body.velocity = velocity * new Vector2(1, -1);
            } else if (Approximately(contactPoint.y, bounds.max.y)) {
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
    }

    bool Approximately(float valueA, float valueB) {
        float diff = Mathf.Abs(valueA - valueB);
        return diff <= 0.01f;
    }
}


        

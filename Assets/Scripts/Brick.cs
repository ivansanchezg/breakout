using UnityEngine;

public class Brick : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Destroy() {
        Destroy(gameObject);
    }

    public void SetColor(Color color) {
        spriteRenderer.color = color;
    }
}
